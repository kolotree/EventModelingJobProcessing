using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI;
using Infrastructure.EventStore.Serialization;
using Shared;

namespace Infrastructure.EventStore
{
    internal sealed class PersistedEventSource : IPersistedEventSource
    {
        private readonly IEventStoreConnection _connection;

        public PersistedEventSource(IEventStoreConnection connection)
        {
            _connection = connection;
        }
        
        public async Task SubscribeTo<T>(
            Func<T, Task> eventHandler,
            CancellationToken cancellationToken = default) where T : IEvent
        {
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            var subscription = await _connection.ConnectToPersistentSubscriptionAsync(
                $"$et-{typeof(T).Name}",
                "test",
                async (s, resolvedEvent) =>
                {
                    if (resolvedEvent.IsResolved)
                    {
                        await eventHandler((T) resolvedEvent.Event.ToEvent());
                    }
                    
                    s.Acknowledge(resolvedEvent);
                },
                (_, __, ___) => subscriptionDroppedCancellationTokenSource.Cancel());

            try
            {
                WaitHandle.WaitAny(new[]
                {
                    cancellationToken.WaitHandle,
                    subscriptionDroppedCancellationTokenSource.Token.WaitHandle
                });
            }
            finally
            {
                subscription.Stop(TimeSpan.FromSeconds(10));
            } 
        }
    }
}