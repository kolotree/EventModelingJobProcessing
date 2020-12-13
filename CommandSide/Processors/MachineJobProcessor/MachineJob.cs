using Abstractions;
using Shared;

namespace MachineJobProcessor
{
    internal sealed class MachineJob : AggregateRoot
    {
        public static Optional<MachineJob> NewStartedJobFrom(StartNewMachineJobCommand c) =>
            c.ToNewMachineJobStarted()
                .Map(newMachineJobStarted =>
                {
                    var machineJob = new MachineJob();
                    machineJob.ApplyChange(newMachineJobStarted);
                    return machineJob;
                });

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