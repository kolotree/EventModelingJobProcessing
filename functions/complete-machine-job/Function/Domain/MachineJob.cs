using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class MachineJob : Stream
    {
        private bool _isCompleted;
        
        public void Execute(Command c)
        {
            if (!_isCompleted)
            {
                ApplyChange(c.ToMachineJobComplete().ToEventEnvelope());
            }
        }

        protected override void When(EventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Type)
            {
                case nameof(NewMachineJobStarted):
                    var newMachineJobStarted = eventEnvelope.Deserialize<NewMachineJobStarted>();
                    SetIdentity(StreamId.AssembleFor<MachineJob>(
                        newMachineJobStarted.FactoryId,
                        newMachineJobStarted.MachineId,
                        newMachineJobStarted.JobId));
                    _isCompleted = false;
                    break;
                case nameof(MachineJobCompleted):
                    _isCompleted = true;
                    break;
            }
        }
    }
}