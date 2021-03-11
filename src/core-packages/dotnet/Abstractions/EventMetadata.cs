using System.Collections.Generic;

namespace JobProcessing.Abstractions
{
    public sealed class EventMetadata : ValueObject
    {
        public string EventId { get; }
        public string CorrelationId { get; }
        public string CausationId { get; }

        public EventMetadata(
            string eventId,
            string correlationId,
            string causationId)
        {
            EventId = eventId;
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public static EventMetadata From(string eventId, CommandMetadata metadata) => 
            new(eventId, metadata.CorrelationId, metadata.Id);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CorrelationId;
            yield return CausationId;
        }

        public override string ToString()
        {
            return $"{nameof(EventId)}: {EventId}, {nameof(CorrelationId)}: {CorrelationId}, {nameof(CausationId)}: {CausationId}";
        }
    }
}