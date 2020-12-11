using System;
using System.Collections.Generic;
using Shared;

namespace Abstractions
{
    public abstract class AggregateRoot
    {
        private Optional<StreamId> _optionalStreamId = Optional<StreamId>.None;

        public StreamId StreamId => _optionalStreamId
            .Ensure(m => m.HasValue,
                () => throw new InvalidOperationException("Aggregate Id needs to be set during object creation in order to use the aggregate."))
            .Value;
        
        public long Version { get; private set; } = -1;

        public long OriginalVersion => Version - UncommittedEvents.Count;
        
        private readonly List<IEvent> _uncommittedEvents = new List<IEvent>();
        public IReadOnlyList<IEvent> UncommittedEvents => _uncommittedEvents;

        protected void SetIdentity(StreamId streamId) =>
            _optionalStreamId = Optional<StreamId>.From(streamId);

        protected abstract void When(IEvent e);
        
        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }
        
        public AggregateRoot ApplyAll(IReadOnlyList<IEvent> events)
        {
            foreach (var e in events)
            {
                ApplyChange(e, false);
            }
            
            return this;
        }
        
        protected AggregateRoot ApplyChange(IEvent e)
        {
            ApplyChange(e, true);
            return this;
        }
        
        private void ApplyChange(IEvent e, bool isNew)
        {
            When(e);
            IncrementedVersion();
            if (isNew)
            {
                _uncommittedEvents.Add(e);
            }
        }
        
        private void IncrementedVersion() => ++Version;
    }
}