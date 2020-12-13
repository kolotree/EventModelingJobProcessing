using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI;
using Infrastructure.EventStore.Serialization;
using Newtonsoft.Json;
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

        public async Task SubscribeToView<T>(Func<T, Task> viewHandler, CancellationToken cancellationToken = default) where T : IView
        {
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            var subscription = await _connection.ConnectToPersistentSubscriptionAsync(
                typeof(T).Name,
                "test",
                async (s, resolvedEvent) =>
                {
                    if (resolvedEvent.IsResolved)
                    {
                        await viewHandler(JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(resolvedEvent.Event.Data)));
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