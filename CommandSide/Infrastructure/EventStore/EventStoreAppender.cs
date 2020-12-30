using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using EventStore.Client;
using Infrastructure.EventStore.Serialization;
using Shared;

namespace Infrastructure.EventStore
{
    internal sealed class EventStoreAppender
    {
        private readonly EventStoreClient _eventStoreClient;
        
        public EventStoreAppender(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }
        
        public async Task<IReadOnlyList<IEvent>> AsyncLoadAllEventsFor(StreamId streamId)
        {
            var result = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamId, StreamPosition.Start);
            var resolvedEvents = await result.ToListAsync();
            return resolvedEvents.Select(e => e.Event.ToEvent()).ToList();
        }

        public async Task ConditionalAppendAsync(
            StreamId streamId,
            IReadOnlyList<IEvent> events,
            long expectedVersion)
        {
            if (events.Count > 0)
            {
                var results = await _eventStoreClient.ConditionalAppendToStreamAsync(
                    streamId,
                    StreamRevision.FromInt64(expectedVersion),
                    events.Select(e => e.ToEventData()));

                switch (results.Status)
                {
                    case ConditionalWriteStatus.Succeeded:
                        break;
                    case ConditionalWriteStatus.VersionMismatch:
                        throw new VersionMismatchException(streamId, expectedVersion);
                    case ConditionalWriteStatus.StreamDeleted:
                        throw new StreamDeletedException(streamId);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(results.Status), results.Status.ToString());
                }
            }
        }
        
        public Task AppendAsync(
            StreamId streamId,
            IReadOnlyList<IEvent> events)
        {
            if (events.Count > 0)
            {
                return _eventStoreClient.AppendToStreamAsync(
                    streamId, StreamState.Any, events.Select(e => e.ToEventData()));
            }
            
            return Task.CompletedTask;
        }
    }
}