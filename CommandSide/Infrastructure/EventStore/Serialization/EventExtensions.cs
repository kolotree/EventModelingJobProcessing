using System;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Shared;

namespace Infrastructure.EventStore.Serialization
{
    internal static class EventExtensions
    {
        public static EventData ToEventData(this IEvent e) =>
            new(
                Guid.NewGuid(), 
                e.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)),
                EventMetaData.Of(e).ToByteArray());
    }
}