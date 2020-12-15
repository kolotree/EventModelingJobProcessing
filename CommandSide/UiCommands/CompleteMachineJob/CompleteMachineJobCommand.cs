using Abstractions;
using Shared;

namespace CompleteMachineStoppage
{
    public sealed class CompleteMachineJobCommand : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public string JobId { get; }

        public CompleteMachineJobCommand(string factoryId, string machineId, string jobId)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            JobId = jobId;
        }
        
        public StreamId JobStream => StreamId.AssembleFor<MachineJob>(FactoryId, MachineId, JobId);
        
        public MachineJobCompleted ToMachineJobComplete() => new(
            FactoryId,
            MachineId,
            JobId);
    }
}