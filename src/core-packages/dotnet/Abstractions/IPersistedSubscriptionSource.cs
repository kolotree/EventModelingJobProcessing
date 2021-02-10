using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobProcessing.Abstractions
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

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(StreamName)}: {StreamName}");
            stringBuilder.AppendLine($"{nameof(SubscriptionGroupName)}: {SubscriptionGroupName}");
            stringBuilder.AppendLine($"{nameof(ProjectStartingFromEventPosition)}: {ProjectStartingFromEventPosition}");
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}