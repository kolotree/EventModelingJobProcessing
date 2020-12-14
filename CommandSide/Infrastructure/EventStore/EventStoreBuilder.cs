using System;
using System.Net;
using Abstractions;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Log;
using EventStore.ClientAPI.Projections;

namespace Infrastructure.EventStore
{
    public sealed class EventStoreBuilder : IDisposable
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public EventStoreBuilder(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public static EventStoreBuilder NewUsing(
            string eventStoreConnectionString)
        {
            var connection = EventStoreConnection.Create(
                ConnectionSettings.Create()
                    .KeepReconnecting()
                    .KeepRetrying(),
                new Uri(eventStoreConnectionString));
            
            connection.ConnectAsync().Wait();
            
            return new EventStoreBuilder(connection);
        }

        public IStore NewStore() => new Store(_eventStoreConnection);
        
        public IPersistedSubscriptionSource NewPersistedSubscriptionSource() 
            => new PersistedSubscriptionSource(
                _eventStoreConnection,
                new ProjectionsManager(
                    new ConsoleLogger(),
                    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2113),
                    TimeSpan.FromMilliseconds(5000)));
        
        public void Dispose()
        {
            _eventStoreConnection?.Dispose();
        }
    }
}