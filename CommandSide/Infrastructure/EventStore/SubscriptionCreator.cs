using System;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.Client;

namespace Infrastructure.EventStore
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
            SubscriptionRequest subscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            await CreateSubscriptionFor(subscriptionRequest);
            await _next.SubscribeTo(subscriptionRequest, viewHandler, cancellationToken);
        }

        private async Task CreateSubscriptionFor(SubscriptionRequest request)
        {
            try
            {
                await _client.CreateAsync(
                    request.StreamName,
                    request.SubscriptionGroupName,
                    new PersistentSubscriptionSettings(
                        false,
                        StreamPosition.FromInt64(request.ProjectStartingFromEventPosition)));
            }
            catch (InvalidOperationException ex)
            {
                if (!ex.Message.Contains("already exists"))
                {
                    throw;
                }
            }
        }
    }
}