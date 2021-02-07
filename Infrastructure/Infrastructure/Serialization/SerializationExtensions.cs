using JobProcessing.Abstractions;
using Newtonsoft.Json;

namespace JobProcessing.Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static T Deserialize<T>(this EventEnvelope eventEnvelope) => 
            JsonConvert.DeserializeObject<T>(eventEnvelope.Data);

        public static EventEnvelope ToEventEnvelope(this IEvent @event) =>
            new(
                @event.GetType().Name,
                JsonConvert.SerializeObject(@event),
                "");
    }
}