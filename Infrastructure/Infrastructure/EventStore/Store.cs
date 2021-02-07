using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Client;
using JobProcessing.Abstractions;

namespace JobProcessing.Infrastructure.EventStore
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
            var eventEnvelopes = await _eventStoreAppender.AsyncLoadAllEventEnvelopesFor(streamId);
            if (eventEnvelopes.Count == 0)
            {
                throw new StreamDoesntExist(streamId);
            }
            
            return ReconstructStreamFrom<T>(eventEnvelopes);
        }
        
        private static T ReconstructStreamFrom<T>(IReadOnlyList<EventEnvelope> eventEnvelopes) where T : Stream, new()
        {
            var stream = new T();
            stream.ApplyAll(eventEnvelopes);
            return stream;
        }

        public async Task SaveChanges<T>(T stream) where T : Stream
        {
            await _eventStoreAppender.ConditionalAppendAsync(stream.StreamId, stream.UncommittedEventEnvelopes, stream.OriginalVersion);
            stream.ClearUncommittedEvents();
        }

        public Task AppendTo<T>(T stream) where T : IStream => 
            _eventStoreAppender.AppendAsync(stream.StreamId, stream.UncommittedEventEnvelopes);
    }
}