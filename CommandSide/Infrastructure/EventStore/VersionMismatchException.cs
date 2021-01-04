using System;
using Abstractions;

namespace Infrastructure.EventStore
{
    internal sealed class VersionMismatchException : Exception
    {
        public VersionMismatchException(StreamId streamId, long expectedVersion)
            : base($"Expected version for stream '{streamId}' is {expectedVersion}, but there is more events in the stream.")
        {
        }
    }
}