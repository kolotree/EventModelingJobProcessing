using System;
using Abstractions;
using Shared;

namespace MachineJobProcessor
{
    internal sealed class StartNewMachineJobCommand : ICommand
    {
        public MachineJobProcessingView MachineJobProcessingView { get; }

        public StartNewMachineJobCommand(MachineJobProcessingView machineJobProcessingView)
        {
            MachineJobProcessingView = machineJobProcessingView;
        }
        
        public Optional<NewMachineJobStarted> ToNewMachineJobStarted()
        {
            if (MachineJobProcessingView.MachineStartedTime.HasValue &&
                !MachineJobProcessingView.JobState.HasValue)
            {
                return new NewMachineJobStarted(
                    MachineJobProcessingView.FactoryId,
                    MachineJobProcessingView.MachineId,
                    MachineJobProcessingView.MachineStartedTime.Value.Ticks.ToString(),
                    MachineJobProcessingView.MachineStartedTime.Value);
            }
            
            return Optional<NewMachineJobStarted>.None;
        }
    }
}