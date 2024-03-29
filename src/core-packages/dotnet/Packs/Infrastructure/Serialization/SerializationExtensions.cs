using System;
using EventStore.Client;
using JobProcessing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JobProcessing.Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static T Deserialize<T>(this EventEnvelope eventEnvelope) where T : IEvent => 
            JsonConvert.DeserializeObject<T>(eventEnvelope.Data);

        public static EventEnvelope ToEventEnvelopeUsing(this IEvent @event, CommandMetadata metadata) =>
            new(
                @event.GetType().Name,
                JsonConvert.SerializeObject(@event),
                EventMetadata.From(Uuid.NewUuid().ToString(), metadata));

        internal static string Serialize(this EventMetadata eventMetadata) =>
            JsonConvert.SerializeObject(eventMetadata, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new EventMetadataNamingStrategy()},
                Formatting = Formatting.Indented
            });

        internal static EventMetadata? DeserializeEventMetadata(this string obj) =>
            JsonConvert.DeserializeObject<EventMetadata>(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new EventMetadataNamingStrategy()},
                Formatting = Formatting.Indented
            });

        private sealed class EventMetadataNamingStrategy : CamelCaseNamingStrategy
        {
            protected override string ResolvePropertyName(string name) => $"${base.ResolvePropertyName(name)}";
        }
    }
}