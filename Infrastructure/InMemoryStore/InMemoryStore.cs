using System.Collections.Generic;
using System.Threading.Tasks;
using JobProcessing.Abstractions;

namespace JobProcessing.InMemoryStore
{
    public sealed class InMemoryStore : IStore
    {
        private readonly Dictionary<StreamId, List<EventEnvelope>> _cache = new();
        private readonly List<EventEnvelope> _producedEventEnvelopes = new();
        
        public InMemoryStore Given(StreamId streamId, params EventEnvelope[] newStreamEvents)
        {
            if (!_cache.TryGetValue(streamId, out var streamEvents))
            {
                streamEvents = new List<EventEnvelope>();
                _cache.Add(streamId, streamEvents);
            }
            
            streamEvents.AddRange(newStreamEvents);
            return this;
        }

        public IReadOnlyList<EventEnvelope> ProducedEventEnvelopes => _producedEventEnvelopes;
        
        public Task<T> Get<T>(StreamId streamId) where T : Stream, new()
        {
            if (_cache.TryGetValue(streamId, out var streamEvents))
            {
                var stream = new T();
                stream.ApplyAll(streamEvents);
                return Task.FromResult(stream);
            }

            throw new StreamDoesntExist(streamId);
        }

        public Task SaveChanges<T>(T stream) where T : Stream
        {
            if (!_cache.TryGetValue(stream.StreamId, out var streamEvents))
            {
                streamEvents = new List<EventEnvelope>();
                _cache.Add(stream.StreamId, streamEvents);
            }
            
            streamEvents.AddRange(stream.UncommittedEventEnvelopes);
            _producedEventEnvelopes.AddRange(stream.UncommittedEventEnvelopes);
            stream.ClearUncommittedEvents();
            return Task.CompletedTask;
        }

        public Task AppendTo<T>(T stream) where T : IStream
        {
            if (!_cache.TryGetValue(stream.StreamId, out var streamEvents))
            {
                streamEvents = new List<EventEnvelope>();
                _cache.Add(stream.StreamId, streamEvents);
            }
            
            streamEvents.AddRange(stream.UncommittedEventEnvelopes);
            _producedEventEnvelopes.AddRange(stream.UncommittedEventEnvelopes);
            return Task.CompletedTask;
        }
    }
}