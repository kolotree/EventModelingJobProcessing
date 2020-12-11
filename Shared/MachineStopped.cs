using System;
using System.Collections.Generic;

namespace Shared
{
    public sealed class MachineStopped : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string RemoteId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }

        public MachineStopped(
            string factoryId,
            string remoteId,
            string machineId,
            DateTime stoppedAt)
        {
            FactoryId = factoryId;
            RemoteId = remoteId;
            MachineId = machineId;
            StoppedAt = stoppedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return RemoteId;
            yield return MachineId;
            yield return StoppedAt;
        }
    }
}