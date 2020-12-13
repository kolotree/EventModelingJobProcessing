using Abstractions;
using Shared;

namespace MachineJobProcessor
{
    public sealed class MachineJob : AggregateRoot
    {
        public static MachineJob NewStartedJobFrom(StartNewMachineJobCommand c)
        {
            var machineJob = new MachineJob();
            machineJob.ApplyChange(c.ToNewMachineJobStarted());
            return machineJob;
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