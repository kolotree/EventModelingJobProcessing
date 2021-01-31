using System;
using Abstractions;

namespace Infrastructure.EventStore
{
    internal sealed class StreamDoesntExist : Exception
    {
        public StreamDoesntExist(StreamId streamId)
            : base($"Stream '{streamId}' doesn't exist.")
        {
        }
    }
}