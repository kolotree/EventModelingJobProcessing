using System;
using System.Collections.Generic;
using Abstractions;

namespace DetectMachineStop
{
    public sealed class MachineStopped : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }

        public MachineStopped(
            string factoryId,
            string machineId,
            DateTime stoppedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            StoppedAt = stoppedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            yield return StoppedAt;
        }
    }
}