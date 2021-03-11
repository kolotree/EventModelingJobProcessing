using System.Collections.Generic;

namespace JobProcessing.Abstractions
{
    public sealed class CommandMetadata : ValueObject
    {
        public string Id { get; }
        public string CorrelationId { get; }

        public CommandMetadata(
            string id,
            string correlationId)
        {
            Id = id;
            CorrelationId = correlationId;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return CorrelationId;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(CorrelationId)}: {CorrelationId}";
        }
    }
}