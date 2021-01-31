using Abstractions;
using Newtonsoft.Json;

namespace Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static T DeserializeEvent<T>(this EventEnvelope eventEnvelope) => 
            JsonConvert.DeserializeObject<T>(eventEnvelope.Data);

        public static EventEnvelope ToEventEnvelope(this IEvent @event) =>
            new(
                @event.GetType().Name,
                JsonConvert.SerializeObject(@event),
                "");
    }
}