using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using JobProcessing.Abstractions;

namespace JobProcessing.Infrastructure.EventStore
{
    internal sealed class SubscriptionCreator : IPersistedSubscriptionSource
    {
        private readonly EventStorePersistentSubscriptionsClient _client;
        private readonly IPersistedSubscriptionSource _next;

        public SubscriptionCreator(
            EventStorePersistentSubscriptionsClient client,
            IPersistedSubscriptionSource next)
        {
            _client = client;
            _next = next;
        }

        public async Task SubscribeTo<T>(
            PersistedSubscriptionRequest persistedSubscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            await CreateSubscriptionFor(persistedSubscriptionRequest);
            await _next.SubscribeTo(persistedSubscriptionRequest, viewHandler, cancellationToken);
        }

        private async Task CreateSubscriptionFor(PersistedSubscriptionRequest request)
        {
            try
            {
                await _client.CreateAsync(
                    request.StreamName,
                    request.SubscriptionGroupName,
                    new PersistentSubscriptionSettings(
                        true,
                        StreamPosition.FromInt64(request.ProjectStartingFromEventPosition)));
            }
            catch (InvalidOperationException ex)
            {
                if (!ex.Message.Contains("StatusCode=AlreadyExists"))
                {
                    throw;
                }
            }
        }
    }
}