using Abstractions;
using Infrastructure.Serialization;

namespace DetectMachineStart
{
    internal sealed class MachineStoppage : Stream
    {
        private bool _isStarted;
        
        public void Apply(DetectMachineStartCommand c)
        {
            if (!_isStarted)
            {
                ApplyChange(c.ToMachineStarted().ToEventEnvelope());
            }
        }
        
        protected override void When(EventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Type)
            {
                case nameof(MachineStopped):
                    var machineStopped = eventEnvelope.DeserializeEvent<MachineStopped>();
                    SetIdentity(StreamId.AssembleFor<MachineStoppage>(
                        machineStopped.FactoryId,
                        machineStopped.MachineId,
                        machineStopped.StoppedAt.Ticks.ToString()));
                    _isStarted = false;
                    break;
                case nameof(MachineStarted):
                    _isStarted = true;
                    break;
            }
        }
    }
}