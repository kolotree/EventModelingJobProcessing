using System;
using System.Collections.Generic;
using System.Text;
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
        public string ProjectionCode { get; }

        public SubscriptionRequest(
            string streamName,
            string subscriptionGroupName,
            long projectStartingFromEventPosition,
            string projectionCode)
        {
            StreamName = streamName;
            SubscriptionGroupName = subscriptionGroupName;
            ProjectStartingFromEventPosition = projectStartingFromEventPosition;
            ProjectionCode = projectionCode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StreamName;
            yield return SubscriptionGroupName;
            yield return ProjectStartingFromEventPosition;
            yield return ProjectionCode;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(StreamName)}: {StreamName}");
            stringBuilder.AppendLine($"{nameof(SubscriptionGroupName)}: {SubscriptionGroupName}");
            stringBuilder.AppendLine($"{nameof(ProjectStartingFromEventPosition)}: {ProjectStartingFromEventPosition}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(ProjectionCode);
            return stringBuilder.ToString();
        }
    }
}