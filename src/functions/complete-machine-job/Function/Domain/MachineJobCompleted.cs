using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class MachineJobCompleted : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public string JobId { get; }

        public MachineJobCompleted(
            string factoryId,
            string machineId,
            string jobId)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            JobId = jobId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            yield return JobId;
        }
    }
}