using System;
using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class NewMachineJobStarted : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public string JobId { get; }
        public DateTime StartedAt { get; }

        public NewMachineJobStarted(
            string factoryId,
            string machineId,
            string jobId,
            DateTime startedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            JobId = jobId;
            StartedAt = startedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            yield return JobId;
            yield return StartedAt;
        }
    }
}