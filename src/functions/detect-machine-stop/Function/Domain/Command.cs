using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class Command : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }

        public Command(
            string? factoryId,
            string? machineId,
            DateTime? stoppedAt)
        {
            FactoryId = factoryId ?? throw new ArgumentNullException(nameof(FactoryId));
            MachineId = machineId?? throw new ArgumentNullException(nameof(MachineId));
            StoppedAt = stoppedAt?? throw new ArgumentNullException(nameof(StoppedAt));
        }
        
        internal MachineStopped ToMachineStopped() => new(
            FactoryId,
            MachineId,
            StoppedAt);
    }
}