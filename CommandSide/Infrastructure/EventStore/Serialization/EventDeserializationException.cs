using System;

namespace Infrastructure.EventStore.Serialization
{
    internal sealed class EventDeserializationException : Exception
    {
        public EventDeserializationException(
            string serializedEvent,
            Exception innerException)
            : base($"Failed to deserialize event with content: {serializedEvent}.", innerException)
        {
        }
    }
}