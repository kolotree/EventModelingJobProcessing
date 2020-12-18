using Abstractions;
using Shared;

namespace MachineJobProcessor.Domain
{
    internal sealed class MachineJob : AggregateRoot
    {
        public static MachineJob? OptionalNewStartedJobFrom(StartNewMachineJobCommand c)
        {
            var optionalMachineJobStarted = c.ToOptionalNewMachineJobStarted();
            if (optionalMachineJobStarted != null)
            {
                var machineJob = new MachineJob();
                machineJob.ApplyChange(optionalMachineJobStarted);
                return machineJob;
            }

            return null;
        }

        protected override void When(IEvent e)
        {
            switch (e)
            {
                case NewMachineJobStarted newMachineJobStarted:
                    SetIdentity(StreamId.AssembleFor<MachineJob>(
                        newMachineJobStarted.FactoryId,
                        newMachineJobStarted.MachineId,
                        newMachineJobStarted.JobId));
                    break;
            }
        }
    }
}