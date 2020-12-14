using System;
using Abstractions;
using Shared;

namespace MachineJobProcessor
{
    internal sealed class StartNewMachineJobCommand : ICommand
    {
        public MachineJobProcessorView MachineJobProcessorView { get; }
        public Guid NewJobGuid { get; }

        public StartNewMachineJobCommand(
            MachineJobProcessorView machineJobProcessorView,
            Guid newJobGuid)
        {
            MachineJobProcessorView = machineJobProcessorView;
            NewJobGuid = newJobGuid;
        }
        
        public Optional<NewMachineJobStarted> ToOptionalNewMachineJobStarted()
        {
            if (MachineJobProcessorView.MachineStartedTime.HasValue &&
                string.IsNullOrWhiteSpace(MachineJobProcessorView.JobId))
            {
                return new NewMachineJobStarted(
                    MachineJobProcessorView.FactoryId,
                    MachineJobProcessorView.MachineId,
                    NewJobGuid.ToString("N"),
                    MachineJobProcessorView.MachineStartedTime.Value);
            }

            if (!MachineJobProcessorView.MachineStartedTime.HasValue &&
                MachineJobProcessorView.RequestedJobTime.HasValue)
            {
                return new NewMachineJobStarted(
                    MachineJobProcessorView.FactoryId,
                    MachineJobProcessorView.MachineId,
                    NewJobGuid.ToString("N"),
                    MachineJobProcessorView.RequestedJobTime.Value);
            }
            
            return Optional<NewMachineJobStarted>.None;
        }
    }
}