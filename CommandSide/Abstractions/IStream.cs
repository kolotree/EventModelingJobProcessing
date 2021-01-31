using System.Collections.Generic;

namespace Abstractions
{
    public interface IStream
    {
        public StreamId StreamId { get; }
        
        public IReadOnlyList<EventEnvelope> UncommittedEventEnvelopes { get; }
    }
}