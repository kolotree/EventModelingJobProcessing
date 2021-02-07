using System;
using JobProcessing.Abstractions;

namespace JobProcessing.Infrastructure.EventStore
{
    internal sealed class StreamDoesntExist : Exception
    {
        public StreamDoesntExist(StreamId streamId)
            : base($"Stream '{streamId}' doesn't exist.")
        {
        }
    }
}