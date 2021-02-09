using System;
using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class Command : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public IReadOnlyList<DateTime> Timestamps  { get; }

        public Command(
            string? factoryId,
            string? machineId,
            IReadOnlyList<DateTime>? timestamps)
        {
            FactoryId = factoryId ?? throw new ArgumentNullException(nameof(FactoryId));
            MachineId = machineId ?? throw new ArgumentNullException(nameof(MachineId));
            Timestamps = timestamps?? throw new ArgumentNullException(nameof(Timestamps));
        }
        
        internal StreamId MachineCyclesGlobalStream => StreamId.AssembleFor<MachineCycles>(
            FactoryId,
            MachineId);

        internal IReadOnlyList<MachineCyclesDetected> ToMachineCycleDetectedList() =>
            Timestamps.Count > 0
                ? new List<MachineCyclesDetected>
                {
                    new(
                        FactoryId,
                        MachineId,
                        Timestamps)
                }
                : new List<MachineCyclesDetected>();
    }
}