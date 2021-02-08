using System;

namespace JobProcessing.Abstractions
{
    public sealed class VersionMismatchException : Exception
    {
        public VersionMismatchException(StreamId streamId, long expectedVersion)
            : base($"Expected version for stream '{streamId}' is {expectedVersion}, but there is more events in the stream.")
        {
        }
    }
}