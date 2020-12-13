using Abstractions;
using Shared;

namespace DetectMachineStart
{
    internal sealed class MachineStoppage : AggregateRoot
    {
        private bool _isStarted;
        
        public void Apply(DetectMachineStartCommand c)
        {
            if (!_isStarted)
            {
                ApplyChange(c.ToMachineStarted());
            }
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
                    _isStarted = false;
                    break;
                case MachineStarted _:
                    _isStarted = true;
                    break;
            }
        }
    }
}