using System;

namespace JobProcessing.Abstractions
{
    public sealed class StreamDoesntExist : Exception
    {
        public StreamDoesntExist(StreamId streamId)
            : base($"Stream '{streamId}' doesn't exist.")
        {
        }
    }
}