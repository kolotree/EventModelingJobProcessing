using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class NewMachineJobRequest : Stream
    {
        public static NewMachineJobRequest From(Command c, IDateTimeProvider dateTimeProvider)
        {
            var newMachineJobRequest = new NewMachineJobRequest();
            newMachineJobRequest.ApplyChange(c.ToNewMachineJobRequestedUsing(dateTimeProvider).ToEventEnvelopeUsing(c.Metadata));
            return newMachineJobRequest;
        }

        protected override void When(EventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Type)
            {
                case nameof(NewMachineJobRequested):
                    var newMachineJobRequested = eventEnvelope.Deserialize<NewMachineJobRequested>();
                    SetIdentity(StreamId.AssembleFor<NewMachineJobRequest>(
                        newMachineJobRequested.FactoryId,
                        newMachineJobRequested.MachineId,
                        newMachineJobRequested.RequestedJobId));
                    break;
            }
        }
    }
}