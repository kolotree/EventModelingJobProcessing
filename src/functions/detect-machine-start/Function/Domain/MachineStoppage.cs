using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class MachineStoppage : Stream
    {
        private bool _isStarted;
        
        public void Apply(Command c)
        {
            if (!_isStarted)
            {
                ApplyChange(c.ToMachineStarted().ToEventEnvelopeUsing(c.Metadata));
            }
        }
        
        protected override void When(EventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Type)
            {
                case nameof(MachineStopped):
                    var machineStopped = eventEnvelope.Deserialize<MachineStopped>();
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