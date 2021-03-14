using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using JobProcessing.Abstractions;
using static Newtonsoft.Json.JsonConvert;

namespace JobProcessing.Infrastructure.EventStore
{
    internal sealed class PersistedSubscriptionSource : IPersistedSubscriptionSource
    {
        private readonly EventStorePersistentSubscriptionsClient _client;

        public PersistedSubscriptionSource(EventStorePersistentSubscriptionsClient client)
        {
            _client = client;
        }

        public Task SubscribeTo<T>(
            PersistedSubscriptionRequest persistedSubscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            return  SubscribeToStream(persistedSubscriptionRequest, viewHandler, cancellationToken);
        }

        private async Task SubscribeToStream<T>(
            PersistedSubscriptionRequest persistedSubscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            Exception? optionalException = null;
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            using var subscription = await _client.SubscribeAsync(
                persistedSubscriptionRequest.StreamName,
                persistedSubscriptionRequest.SubscriptionGroupName,
                async (s, resolvedEvent, _, _) =>
                {
                    if (resolvedEvent.IsResolved)
                    {
                        var deserializeObject = DeserializeObject<T>(Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span));
                        await viewHandler(deserializeObject);
                    }
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