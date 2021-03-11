using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class Command : ICommand
    {
        public CommandMetadata Metadata { get; }
        public string FactoryId { get; }
        public string MachineId { get; }
        public string JobId { get; }

        public Command(
            CommandMetadata? metadata,
            string? factoryId,
            string? machineId,
            string? jobId)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(Metadata));
            FactoryId = factoryId ?? throw new ArgumentNullException(nameof(factoryId));
            MachineId = machineId?? throw new ArgumentNullException(nameof(machineId));
            JobId = jobId ?? throw new ArgumentNullException(nameof(jobId));
        }
        
        public StreamId JobStream => StreamId.AssembleFor<MachineJob>(FactoryId, MachineId, JobId);
        
        public MachineJobCompleted ToMachineJobComplete() => new(
            FactoryId,
            MachineId,
            JobId);
    }
}