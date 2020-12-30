using System;
using Abstractions;
using EventStore.Client;

namespace Infrastructure.EventStore
{
    public sealed class EventStoreBuilder : IDisposable
    {
        private readonly EventStoreClient _eventStoreClient;
        private readonly EventStorePersistentSubscriptionsClient _eventStorePersistentSubscriptionsClient;

        public EventStoreBuilder(
            EventStoreClient eventStoreClient,
            EventStorePersistentSubscriptionsClient eventStorePersistentSubscriptionsClient)
        {
            _eventStoreClient = eventStoreClient;
            _eventStorePersistentSubscriptionsClient = eventStorePersistentSubscriptionsClient;
        }

        public static EventStoreBuilder NewUsing(
            string eventStoreConnectionString)
        {
            var settings = EventStoreClientSettings.Create(eventStoreConnectionString);
            var eventStoreClient = new EventStoreClient(settings);
            var eventStorePersistentSubscriptionsClient = new EventStorePersistentSubscriptionsClient(settings);
            return new EventStoreBuilder(
                eventStoreClient,
                eventStorePersistentSubscriptionsClient);
        }

        public IStore NewStore() => new Store(_eventStoreClient);

        public IPersistedSubscriptionSource NewPersistedSubscriptionSource()
            => new SubscriptionCreator(
                _eventStorePersistentSubscriptionsClient,
                new PersistedSubscriptionSource(_eventStorePersistentSubscriptionsClient));
        
        public void Dispose()
        {
            _eventStoreClient.Dispose();
        }
    }
}