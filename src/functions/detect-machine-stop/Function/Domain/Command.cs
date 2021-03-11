using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class Command : ICommand
    {
        public CommandMetadata Metadata { get; }
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }
        
        public Command(
            CommandMetadata? metadata,
            string? factoryId,
            string? machineId,
            DateTime? stoppedAt)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(Metadata));
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