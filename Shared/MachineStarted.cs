using System;
using System.Collections.Generic;

namespace Shared
{
    public sealed class MachineStarted : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string RemoteId { get; }
        public string MachineId { get; }
        public DateTime LastStoppedAt { get; }
        public DateTime StartedAt { get; }

        public MachineStarted(
            string factoryId,
            string remoteId,
            string machineId,
            DateTime lastLastStoppedAt,
            DateTime startedAt)
        {
            FactoryId = factoryId;
            RemoteId = remoteId;
            MachineId = machineId;
            LastStoppedAt = lastLastStoppedAt;
            StartedAt = startedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return RemoteId;
            yield return MachineId;
            yield return LastStoppedAt;
            yield return StartedAt;
        }
    }
}