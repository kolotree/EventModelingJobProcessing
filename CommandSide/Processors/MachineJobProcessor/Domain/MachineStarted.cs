using System;
using System.Collections.Generic;
using Abstractions;

namespace MachineJobProcessor.Domain
{
    public sealed class MachineStarted : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime LastStoppedAt { get; }
        public DateTime StartedAt { get; }

        public MachineStarted(
            string factoryId,
            string machineId,
            DateTime lastStoppedAt, 
            DateTime startedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            LastStoppedAt = lastStoppedAt;
            StartedAt = startedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            yield return LastStoppedAt;
            yield return StartedAt;
        }
    }
}