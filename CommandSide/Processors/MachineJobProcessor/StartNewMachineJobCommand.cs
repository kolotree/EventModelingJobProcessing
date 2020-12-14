using System;
using Abstractions;
using Shared;

namespace MachineJobProcessor
{
    internal sealed class StartNewMachineJobCommand : ICommand
    {
        public MachineJobProcessingView MachineJobProcessingView { get; }
        public Guid NewJobGuid { get; }

        public StartNewMachineJobCommand(
            MachineJobProcessingView machineJobProcessingView,
            Guid newJobGuid)
        {
            MachineJobProcessingView = machineJobProcessingView;
            NewJobGuid = newJobGuid;
        }
        
        public Optional<NewMachineJobStarted> ToOptionalNewMachineJobStarted()
        {
            if (MachineJobProcessingView.MachineStartedTime.HasValue &&
                string.IsNullOrWhiteSpace(MachineJobProcessingView.JobId))
            {
                return new NewMachineJobStarted(
                    MachineJobProcessingView.FactoryId,
                    MachineJobProcessingView.MachineId,
                    NewJobGuid.ToString("N"),
                    MachineJobProcessingView.MachineStartedTime.Value);
            }

            if (!MachineJobProcessingView.MachineStartedTime.HasValue &&
                MachineJobProcessingView.RequestedJobTime.HasValue)
            {
                return new NewMachineJobStarted(
                    MachineJobProcessingView.FactoryId,
                    MachineJobProcessingView.MachineId,
                    NewJobGuid.ToString("N"),
                    MachineJobProcessingView.RequestedJobTime.Value);
            }
            
            return Optional<NewMachineJobStarted>.None;
        }
    }
}