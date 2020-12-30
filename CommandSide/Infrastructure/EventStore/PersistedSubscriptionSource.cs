using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.Client;
using static Newtonsoft.Json.JsonConvert;

namespace Infrastructure.EventStore
{
    internal sealed class PersistedSubscriptionSource : IPersistedSubscriptionSource
    {
        private readonly EventStorePersistentSubscriptionsClient _client;

        public PersistedSubscriptionSource(EventStorePersistentSubscriptionsClient client)
        {
            _client = client;
        }

        public Task SubscribeTo<T>(
            SubscriptionRequest subscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            return  SubscribeToStream(subscriptionRequest, viewHandler, cancellationToken);
        }

        private async Task SubscribeToStream<T>(
            SubscriptionRequest subscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            Exception? optionalException = null;
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            using var subscription = await _client.SubscribeAsync(
                subscriptionRequest.StreamName,
                subscriptionRequest.SubscriptionGroupName,
                async (s, resolvedEvent, _, _) =>
                {
                    if (resolvedEvent.IsResolved)
                    {
                        var deserializeObject = DeserializeObject<T>(Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span));
                        await viewHandler(deserializeObject);
                    }

                    s?.Ack(resolvedEvent);
                },
                (_, _, exception) =>
                {
                    optionalException = exception;
                    subscriptionDroppedCancellationTokenSource.Cancel();
                });

            WaitHandle.WaitAny(new[]
            {
                cancellationToken.WaitHandle,
                subscriptionDroppedCancellationTokenSource.Token.WaitHandle
            });

            if (optionalException != null)
            {
                throw new PersistedSubscriptionSourceException(optionalException);
            }
        }
    }
}