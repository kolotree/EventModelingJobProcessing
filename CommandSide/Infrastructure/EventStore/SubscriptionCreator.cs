using System;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI;

namespace Infrastructure.EventStore
{
    internal sealed class SubscriptionCreator : IPersistedSubscriptionSource
    {
        private readonly IEventStoreConnection _connection;
        private readonly IPersistedSubscriptionSource _next;

        public SubscriptionCreator(IEventStoreConnection connection, IPersistedSubscriptionSource next)
        {
            _connection = connection;
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
                await _connection.CreatePersistentSubscriptionAsync(
                    request.StreamName,
                    request.SubscriptionGroupName,
                    PersistentSubscriptionSettings.Create()
                        .StartFrom(request.ProjectStartingFromEventPosition)
                        .ResolveLinkTos(),
                    null);
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