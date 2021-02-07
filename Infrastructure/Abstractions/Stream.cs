using System;
using System.Collections.Generic;

namespace JobProcessing.Abstractions
{
    public abstract class Stream : IStream
    {
        private StreamId? _optionalStreamId;

        public StreamId StreamId => _optionalStreamId == null
            ? throw new InvalidOperationException("Stream Id needs to be set during object creation in order to use the instance.")
            : _optionalStreamId;
        
        public long Version { get; private set; } = -1;

        public long OriginalVersion => Version - UncommittedEventEnvelopes.Count;
        
        private readonly List<EventEnvelope> _uncommittedEventEnvelopes = new();
        public IReadOnlyList<EventEnvelope> UncommittedEventEnvelopes => _uncommittedEventEnvelopes;

        protected void SetIdentity(StreamId streamId) => _optionalStreamId = streamId;

        protected abstract void When(EventEnvelope eventEnvelope);
        
        public void ClearUncommittedEvents()
        {
            _uncommittedEventEnvelopes.Clear();
        }
        
        public void ApplyAll(IReadOnlyList<EventEnvelope> eventEnvelopes)
        {
            foreach (var eventEnvelope in eventEnvelopes)
            {
                ApplyChange(eventEnvelope, false);
            }
        }
        
        protected void ApplyChange(EventEnvelope eventEnvelope)
        {
            ApplyChange(eventEnvelope, true);
        }
        
        private void ApplyChange(EventEnvelope eventEnvelope, bool isNew)
        {
            When(eventEnvelope);
            IncrementedVersion();
            if (isNew)
            {
                _uncommittedEventEnvelopes.Add(@eventEnvelope);
            }
        }
        
        private void IncrementedVersion() => ++Version;
    }
}