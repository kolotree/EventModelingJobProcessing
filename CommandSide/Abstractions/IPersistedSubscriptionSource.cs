using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace Abstractions
{
    public interface IPersistedSubscriptionSource
    {
        Task SubscribeTo<T>(
            SubscriptionRequest subscriptionRequest,
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default);
    }

    public sealed class SubscriptionRequest : ValueObject
    {
        public string StreamName { get; }
        public string SubscriptionGroupName { get; }
        public long ProjectStartingFromEventPosition { get; }

        public SubscriptionRequest(
            string streamName,
            string subscriptionGroupName,
            long projectStartingFromEventPosition)
        {
            StreamName = streamName;
            SubscriptionGroupName = subscriptionGroupName;
            ProjectStartingFromEventPosition = projectStartingFromEventPosition;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StreamName;
            yield return SubscriptionGroupName;
            yield return ProjectStartingFromEventPosition;
        }
    }
}