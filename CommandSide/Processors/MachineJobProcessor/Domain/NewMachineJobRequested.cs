using System;
using System.Collections.Generic;
using Abstractions;

namespace MachineJobProcessor.Domain
{
    public sealed class NewMachineJobRequested : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public string RequestedJobId { get; }
        public DateTime RequestedJobTime { get; }

        public NewMachineJobRequested(
            string factoryId,
            string machineId,
            string requestedJobId,
            DateTime requestedJobTime)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            RequestedJobId = requestedJobId;
            RequestedJobTime = requestedJobTime;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            yield return RequestedJobId;
            yield return RequestedJobTime;
        }
    }
}