using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;
using static System.Text.Encoding;

namespace JobProcessing.Infrastructure.EventStore
{
    internal sealed class EventStoreAppender
    {
        private readonly EventStoreClient _eventStoreClient;
        
        public EventStoreAppender(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }
        
        public async Task<IReadOnlyList<EventEnvelope>> AsyncLoadAllEventEnvelopesFor(StreamId streamId)
        {
            var result = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamId, StreamPosition.Start);
            var resolvedEvents = await result.ToListAsync();
            return resolvedEvents.Select(e => 
                new EventEnvelope(
                    e.Event.EventType,
                    UTF8.GetString(e.Event.Data.Span),
                    UTF8.GetString(e.Event.Metadata.Span).DeserializeEventMetadata() ?? EventMetadata.Empty()))
                .ToList();
        }

        public async Task ConditionalAppendAsync(
            StreamId streamId,
            IReadOnlyList<EventEnvelope> eventEnvelopes,
            long expectedVersion)
        {
            if (eventEnvelopes.Count > 0)
            {
                var results = await _eventStoreClient.ConditionalAppendToStreamAsync(
                    streamId,
                    StreamRevision.FromInt64(expectedVersion),
                    eventEnvelopes.Select(ee => new EventData(
                        Uuid.NewUuid(), 
                        ee.Type,
                        UTF8.GetBytes(ee.Data),
                        UTF8.GetBytes(ee.Metadata.Serialize()))));

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
            IReadOnlyList<EventEnvelope> eventEnvelopes)
        {
            if (eventEnvelopes.Count > 0)
            {
                return _eventStoreClient.AppendToStreamAsync(
                    streamId, StreamState.Any, eventEnvelopes.Select(ee => new EventData(
                        Uuid.NewUuid(), 
                        ee.Type,
                        UTF8.GetBytes(ee.Data),
                        UTF8.GetBytes(ee.Metadata.Serialize()))));
            }
            
            return Task.CompletedTask;
        }
    }
}