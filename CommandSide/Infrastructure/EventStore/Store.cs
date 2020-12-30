using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using EventStore.Client;
using Shared;

namespace Infrastructure.EventStore
{
    internal sealed class Store : IStore
    {
        private readonly EventStoreAppender _eventStoreAppender;

        public Store(EventStoreClient client)
        {
            _eventStoreAppender = new EventStoreAppender(client);
        }
        
        public async Task<T> Get<T>(StreamId streamId) where T : AggregateRoot, new()
        {
            var events = await _eventStoreAppender.AsyncLoadAllEventsFor(streamId);
            if (events.Count == 0)
            {
                throw new StreamDoesntExist(streamId);
            }
            
            return ReconstructAggregateFrom<T>(events);
        }
        
        private static T ReconstructAggregateFrom<T>(IReadOnlyList<IEvent> events) where T : AggregateRoot, new()
        {
            var aggregateRoot = new T();
            aggregateRoot.ApplyAll(events);
            return aggregateRoot;
        }

        public async Task SaveChanges<T>(T aggregateRoot) where T : AggregateRoot
        {
            await _eventStoreAppender.ConditionalAppendAsync(aggregateRoot.StreamId, aggregateRoot.UncommittedEvents, aggregateRoot.OriginalVersion);
            aggregateRoot.ClearUncommittedEvents();
        }

        public Task AppendTo<T>(T stream) where T : IStream => 
            _eventStoreAppender.AppendAsync(stream.StreamId, stream.UncommittedEvents);
    }
}