﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI;
using Shared;

namespace Infrastructure.EventStore
{
    internal sealed class Store : IStore
    {
        private readonly EventStoreAppender _eventStoreAppender;

        public Store(IEventStoreConnection connectionProvider)
        {
            _eventStoreAppender = new EventStoreAppender(connectionProvider);
        }
        
        public async Task<T> Get<T>(StreamId streamId) where T : AggregateRoot, new()
        {
            var events = await _eventStoreAppender.AsyncLoadAllEventsFor(streamId);
            if (events.Count == 0)
            {
                throw new StreamDoesntExist(streamId);
            }
            
            return ReconstructAggregateFrom<T>(events);
        }
        
        private static T ReconstructAggregateFrom<T>(IReadOnlyList<IEvent> events) where T : AggregateRoot, new()
        {
            var aggregateRoot = new T();
            aggregateRoot.ApplyAll(events);
            return aggregateRoot;
        }

        public async Task SaveChanges<T>(T aggregateRoot) where T : AggregateRoot
        {
            await _eventStoreAppender.AppendAsync(aggregateRoot.StreamId, aggregateRoot.UncommittedEvents, aggregateRoot.OriginalVersion);
            aggregateRoot.ClearUncommittedEvents();
        }
    }
}