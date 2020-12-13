using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI;
using Shared;
using static Newtonsoft.Json.JsonConvert;

namespace Infrastructure.EventStore
{
    internal sealed class PersistedSubscriptionSource : IPersistedSubscriptionSource
    {
        private readonly IEventStoreConnection _connection;

        public PersistedSubscriptionSource(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public Task SubscribeToEventType<T>(Func<T, Task> eventHandler, CancellationToken cancellationToken = default) 
            where T : IEvent => SubscribeTo(eventHandler, cancellationToken);

        public Task SubscribeToView<T>(Func<T, Task> viewHandler, CancellationToken cancellationToken = default) 
            where T : IView => SubscribeTo(viewHandler, cancellationToken);

        private async Task SubscribeTo<T>(
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            Optional<Exception> optionalException;
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            var subscription = await _connection.ConnectToPersistentSubscriptionAsync(
                typeof(T).Name,
                "test",
                async (s, resolvedEvent) =>
                {
                    if (resolvedEvent.IsResolved)
                    {
                        var deserializedView = DeserializeObject<T>(Encoding.UTF8.GetString(resolvedEvent.Event.Data));
                        await viewHandler(deserializedView);
                    }
                    
                    s.Acknowledge(resolvedEvent);
                },
                (_, __, exception) =>
                {
                    optionalException = exception;
                    subscriptionDroppedCancellationTokenSource.Cancel();
                });

            try
            {
                WaitHandle.WaitAny(new[]
                {
                    cancellationToken.WaitHandle,
                    subscriptionDroppedCancellationTokenSource.Token.WaitHandle
                });

                if (optionalException.HasValue)
                {
                    throw optionalException.Value;
                }
            }
            finally
            {
                subscription.Stop(TimeSpan.FromSeconds(10));
            } 
        }
    }
}