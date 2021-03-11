using System;
using JobProcessing.Abstractions;

namespace Processor.Domain
{
    internal sealed class StartNewMachineJobCommand : ICommand
    {
        public CommandMetadata Metadata { get; }
        public MachineJobProcessorView MachineJobProcessorView { get; }
        public Guid NewJobGuid { get; }


        public StartNewMachineJobCommand(
            CommandMetadata metadata,
            MachineJobProcessorView machineJobProcessorView,
            Guid newJobGuid)
        {
            Metadata = metadata;
            MachineJobProcessorView = machineJobProcessorView;
            NewJobGuid = newJobGuid;
        }

        public NewMachineJobStarted? ToOptionalNewMachineJobStarted() =>
            MachineJobProcessorView.ToOptionalNewMachineJobStartedUsing(NewJobGuid);
    }
}