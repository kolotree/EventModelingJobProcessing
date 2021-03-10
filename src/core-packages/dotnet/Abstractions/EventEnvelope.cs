using System.Collections.Generic;

namespace JobProcessing.Abstractions
{
    public sealed class EventEnvelope : ValueObject
    {
        public string Type { get; }
        public string Data { get; }
        public EventMetadata Metadata { get; }

        public EventEnvelope(string type, string data, EventMetadata metadata)
        {
            Type = type;
            Data = data;
            Metadata = metadata;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Data;
            yield return Metadata;
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Data)}: {Data}, {nameof(Metadata)}: {Metadata}";
        }
    }
}