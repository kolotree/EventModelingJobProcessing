using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobProcessing.Abstractions
{
    public interface IClientSubscriptionSource
    {
        Task SubscribeUsing(
            ClientSubscriptionRequest clientSubscriptionRequest,
            Func<EventEnvelope, GlobalPosition, Task> eventEnveloperHandler,
            CancellationToken cancellationToken = default);
    }

    public sealed class ClientSubscriptionRequest : ValueObject
    {
        public GlobalPosition StartPosition { get; }
        public string[] EventTypes { get; }

        public ClientSubscriptionRequest(
            GlobalPosition startPosition,
            params string[] eventTypes)
        {
            StartPosition = startPosition;
            EventTypes = eventTypes;
        }

        public string EventTypesRegex => string.Join("|", EventTypes);
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartPosition;
            foreach (var eventType in EventTypes) yield return eventType;
        }
    }
}