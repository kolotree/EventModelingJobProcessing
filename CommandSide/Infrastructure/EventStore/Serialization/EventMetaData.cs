using System;
using System.Text;
using EventStore.Client;
using Newtonsoft.Json;
using Shared;

namespace Infrastructure.EventStore.Serialization
{
    internal sealed class EventMetaData
    {
        public string FullEventType { get; }

        public EventMetaData(string fullEventType)
        {
            FullEventType = fullEventType;
        }
        
        public static EventMetaData Of(IEvent e) =>
            new(e.GetType().AssemblyQualifiedName);

        public static EventMetaData EventMetaDataFrom(EventRecord eventRecord)
        {
            var serializedEventMetaDataString = Encoding.UTF8.GetString(eventRecord.Metadata.Span);
            return EventMetaDataFrom(serializedEventMetaDataString);
        }

        public static EventMetaData EventMetaDataFrom(string serializedEventMetaDataString)
        {
            try
            {
                return JsonConvert.DeserializeObject<EventMetaData>(serializedEventMetaDataString);
            }
            catch (Exception ex)
            {
                throw new EventMetaDataDeserializationException(serializedEventMetaDataString, ex);
            }
        }

        public byte[] ToByteArray() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}