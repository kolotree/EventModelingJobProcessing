using System.Collections.Generic;

namespace JobProcessing.Abstractions
{
    public sealed class EventMetadata : ValueObject
    {
        public string CorrelationId { get; }
        public string CausationId { get; }

        private EventMetadata(
            string correlationId,
            string causationId)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public static EventMetadata From(ICommand command) => new(command.CorrelationId, command.Id);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CorrelationId;
            yield return CausationId;
        }

        public override string ToString()
        {
            return $"{nameof(CorrelationId)}: {CorrelationId}, {nameof(CausationId)}: {CausationId}";
        }
    }
}