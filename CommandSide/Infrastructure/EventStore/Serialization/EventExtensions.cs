using System.Text;
using EventStore.Client;
using Newtonsoft.Json;
using Shared;

namespace Infrastructure.EventStore.Serialization
{
    internal static class EventExtensions
    {
        public static EventData ToEventData(this IEvent e) =>
            new(
                Uuid.NewUuid(), 
                e.GetType().Name,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)),
                EventMetaData.Of(e).ToByteArray());
    }
}