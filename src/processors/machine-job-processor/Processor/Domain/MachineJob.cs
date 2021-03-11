using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Processor.Domain
{
    internal sealed class MachineJob : Stream
    {
        public static MachineJob? OptionalNewStartedJobFrom(StartNewMachineJobCommand c)
        {
            var optionalMachineJobStarted = c.ToOptionalNewMachineJobStarted();
            if (optionalMachineJobStarted != null)
            {
                var machineJob = new MachineJob();
                machineJob.ApplyChange(optionalMachineJobStarted.ToEventEnvelopeUsing(c.Metadata));
                return machineJob;
            }

            return null;
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
                    break;
            }
        }
    }
}