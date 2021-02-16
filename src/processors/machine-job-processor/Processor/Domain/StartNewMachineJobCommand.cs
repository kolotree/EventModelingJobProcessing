using System;
using JobProcessing.Abstractions;

namespace Processor.Domain
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

        public NewMachineJobStarted? ToOptionalNewMachineJobStarted() =>
            MachineJobProcessorView.ToOptionalNewMachineJobStartedUsing(NewJobGuid);
    }
}