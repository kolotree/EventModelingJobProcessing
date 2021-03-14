using System;
using EventStore.Client;
using JobProcessing.Abstractions;

namespace JobProcessing.Infrastructure.EventStore
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

        public static EventStoreBuilder NewUsing(EventStoreConfiguration configuration)
        {
            var settings = EventStoreClientSettings.Create(configuration.ConnectionString);
            settings.DefaultCredentials = configuration.UserCredentials;
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

        public IClientSubscriptionSource NewClientSubscriptionSource()
            => new ClientSubscriptionSource(_eventStoreClient);
        
        public void Dispose()
        {
            _eventStoreClient.Dispose();
        }
    }
}