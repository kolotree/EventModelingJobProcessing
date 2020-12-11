using System;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Shared;

namespace Infrastructure.EventStore.Serialization
{
    internal static class EventDeserializationExtensions
    {
        public static IEvent ToEvent(this RecordedEvent recordedEvent)
        {
            var serializedEventString = Encoding.UTF8.GetString(recordedEvent.Data);
            var eventMetaData = EventMetaData.EventMetaDataFrom(recordedEvent);
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
                        Type.GetType(eventMetaData.FullEventType));
            }
            catch (Exception ex)
            {
                throw new EventDeserializationException(serializedEventData, ex);
            }
        }
    }
}