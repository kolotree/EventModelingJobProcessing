using System.Collections.Generic;
using Shared;

namespace Abstractions
{
    public interface IStream
    {
        public StreamId StreamId { get; }
        
        public IReadOnlyList<IEvent> UncommittedEvents { get; }
    }
}