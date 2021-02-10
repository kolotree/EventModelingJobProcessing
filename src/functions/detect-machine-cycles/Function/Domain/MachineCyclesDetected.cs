using System;
using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class MachineCyclesDetected : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public IReadOnlyList<DateTime> Timestamps  { get; }
        
        public MachineCyclesDetected(string factoryId, string machineId, IReadOnlyList<DateTime> timestamps)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            Timestamps = timestamps;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            foreach (var timestamp in Timestamps) yield return timestamp;
        }
    }
}