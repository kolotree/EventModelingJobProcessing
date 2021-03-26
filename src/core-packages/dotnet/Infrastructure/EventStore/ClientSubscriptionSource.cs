using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace JobProcessing.Infrastructure.EventStore
{
    internal sealed class ClientSubscriptionSource : IClientSubscriptionSource
    {
        private readonly EventStoreClient _eventStoreClient;

        public ClientSubscriptionSource(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }
        
        public async Task SubscribeUsing(
            ClientSubscriptionRequest clientSubscriptionRequest, 
            Func<EventEnvelope, GlobalPosition, Task> eventEnveloperHandler,
            CancellationToken cancellationToken = default)
        {
            Exception? optionalException = null;
            var subscriptionDroppedCancellationTokenSource = new CancellationTokenSource();
            using var streamSubscription = await _eventStoreClient.SubscribeToAllAsync(
                new Position(clientSubscriptionRequest.StartPosition.Part1, clientSubscriptionRequest.StartPosition.Part2),
                async (_, resolvedEvent, _) =>
                {
                    var eventEnvelope = new EventEnvelope(
                        resolvedEvent.Event.EventType,
                        Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span),
                        Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.Span).DeserializeEventMetadata()
                            ?? throw new InvalidOperationException($"Event metadata not provided: {resolvedEvent.Event.EventId}"));
                    await eventEnveloperHandler(
                        eventEnvelope,
                        GlobalPosition.Of(
                            resolvedEvent.OriginalPosition!.Value.CommitPosition,
                            resolvedEvent.OriginalPosition!.Value.PreparePosition));
                },
                false,
                (_, _, exception) =>
                {
                    optionalException = exception;
                    subscriptionDroppedCancellationTokenSource.Cancel();
                }, 
                new SubscriptionFilterOptions(EventTypeFilter.RegularExpression(clientSubscriptionRequest.EventTypesRegex)),
                cancellationToken: subscriptionDroppedCancellationTokenSource.Token);

            WaitHandle.WaitAny(new[]
            {
                cancellationToken.WaitHandle,
                subscriptionDroppedCancellationTokenSource.Token.WaitHandle
            });

            if (optionalException != null)
            {
                throw new PersistedSubscriptionSourceException(optionalException);
            }
        }
    }
}