using Abstractions;
using Shared;

namespace DetectMachineStop
{
    internal sealed class MachineStoppage : Stream
    {
        public static MachineStoppage NewOf(DetectMachineStopCommand c)
        {
            var machineStoppage = new MachineStoppage();
            machineStoppage.ApplyChange(c.ToMachineStopped());
            return machineStoppage;
        }
        
        protected override void When(IEvent e)
        {
            switch (e)
            {
                case MachineStopped machineStopped:
                    SetIdentity(StreamId.AssembleFor<MachineStoppage>(
                        machineStopped.FactoryId,
                        machineStopped.MachineId,
                        machineStopped.StoppedAt.Ticks.ToString()));
                    break;
            }
        }
    }
}