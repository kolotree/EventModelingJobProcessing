﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using static Newtonsoft.Json.JsonConvert;

namespace Infrastructure.EventStore
{
    internal sealed class PersistedSubscriptionSource : IPersistedSubscriptionSource
    {
        private readonly IEventStoreConnection _connection;
        private readonly ProjectionCreator _projectionCreator;

        public PersistedSubscriptionSource(
            IEventStoreConnection connection,
            ProjectionsManager projectionsManager)
        {
            _connection = connection;
            _projectionCreator = new ProjectionCreator(projectionsManager);
        }

        public async Task SubscribeTo<T>(
            SubscriptionRequest subscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            await _projectionCreator.CreateProjectionFrom(subscriptionRequest);
            await SubscribeToStream(subscriptionRequest, viewHandler, cancellationToken);
        }

        private async Task SubscribeToStream<T>(
            SubscriptionRequest subscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default)
        {
            Optional<Exception> optionalException = Optional<Exception>.None;
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            var subscription = await _connection.ConnectToPersistentSubscriptionAsync(
                subscriptionRequest.StreamName,
                subscriptionRequest.SubscriptionGroupName,
                async (s, resolvedEvent) =>
                {
                    if (resolvedEvent.IsResolved)
                    {
                        var deserializeObject = DeserializeObject<T>(Encoding.UTF8.GetString(resolvedEvent.Event.Data));
                        await viewHandler(deserializeObject);
                    }
                    
                    s.Acknowledge(resolvedEvent);
                },
                (_, __, exception) =>
                {
                    optionalException = exception;
                    subscriptionDroppedCancellationTokenSource.Cancel();
                });

            try
            {
                WaitHandle.WaitAny(new[]
                {
                    cancellationToken.WaitHandle,
                    subscriptionDroppedCancellationTokenSource.Token.WaitHandle
                });

                if (optionalException.HasValue)
                {
                    throw new PersistedSubscriptionSourceException(optionalException.Value);
                }
            }
            finally
            {
                subscription.Stop(TimeSpan.FromSeconds(10));
            } 
        }
    }
}