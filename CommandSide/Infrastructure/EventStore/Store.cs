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
        
        public async Task<T> Get<T>(StreamId streamId) where T : Stream, new()
        {
            var events = await _eventStoreAppender.AsyncLoadAllEventsFor(streamId);
            if (events.Count == 0)
            {
                throw new StreamDoesntExist(streamId);
            }
            
            return ReconstructStreamFrom<T>(events);
        }
        
        private static T ReconstructStreamFrom<T>(IReadOnlyList<IEvent> events) where T : Stream, new()
        {
            var stream = new T();
            stream.ApplyAll(events);
            return stream;
        }

        public async Task SaveChanges<T>(T stream) where T : Stream
        {
            await _eventStoreAppender.ConditionalAppendAsync(stream.StreamId, stream.UncommittedEvents, stream.OriginalVersion);
            stream.ClearUncommittedEvents();
        }

        public Task AppendTo<T>(T stream) where T : IStream => 
            _eventStoreAppender.AppendAsync(stream.StreamId, stream.UncommittedEvents);
    }
}