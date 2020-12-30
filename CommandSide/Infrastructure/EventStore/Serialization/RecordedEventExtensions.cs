using System;
using System.Text;
using EventStore.Client;
using Newtonsoft.Json;
using Shared;

namespace Infrastructure.EventStore.Serialization
{
    internal static class EventDeserializationExtensions
    {
        public static IEvent ToEvent(this EventRecord eventRecord)
        {
            var serializedEventString = Encoding.UTF8.GetString(eventRecord.Data.Span);
            var eventMetaData = EventMetaData.EventMetaDataFrom(eventRecord);
            return serializedEventString.ToEventUsing(eventMetaData);
        }

        public static IEvent ToEventUsing(
            this string serializedEventData,
            EventMetaData eventMetaData)
        {
            try
            {
                return (IEvent)JsonConvert
                           .DeserializeObject(
                               serializedEventData,
                               Type.GetType(eventMetaData.FullEventType) ?? throw new ArgumentException(nameof(eventMetaData)))! 
                       ?? throw new ArgumentException(nameof(serializedEventData));
            }
            catch (Exception ex)
            {
                throw new EventDeserializationException(serializedEventData, ex);
            }
        }
    }
}