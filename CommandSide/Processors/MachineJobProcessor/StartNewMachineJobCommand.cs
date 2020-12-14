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
        
        public Optional<NewMachineJobStarted> ToOptionalNewMachineJobStarted()
        {
            if (MachineJobProcessingView.MachineStartedTime.HasValue &&
                string.IsNullOrWhiteSpace(MachineJobProcessingView.JobId))
            {
                return new NewMachineJobStarted(
                    MachineJobProcessingView.FactoryId,
                    MachineJobProcessingView.MachineId,
                    MachineJobProcessingView.MachineStartedTime.Value.Ticks.ToString(),
                    MachineJobProcessingView.MachineStartedTime.Value);
            }

            if (!MachineJobProcessingView.MachineStartedTime.HasValue &&
                MachineJobProcessingView.RequestedJobTime.HasValue)
            {
                return new NewMachineJobStarted(
                    MachineJobProcessingView.FactoryId,
                    MachineJobProcessingView.MachineId,
                    MachineJobProcessingView.RequestedJobTime.Value.Ticks.ToString(),
                    MachineJobProcessingView.RequestedJobTime.Value);
            }
            
            return Optional<NewMachineJobStarted>.None;
        }
    }
}