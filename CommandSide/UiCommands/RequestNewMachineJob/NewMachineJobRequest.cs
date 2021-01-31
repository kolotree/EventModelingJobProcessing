using System;
using Abstractions;
using Infrastructure.Serialization;
using Shared;

namespace RequestNewMachineJob
{
    internal sealed class NewMachineJobRequest : Stream
    {
        public static NewMachineJobRequest From(RequestNewMachineJobCommand c, DateTime jobStartTime)
        {
            var newMachineJobRequest = new NewMachineJobRequest();
            newMachineJobRequest.ApplyChange(c.ToNewMachineJobRequestedUsing(jobStartTime).ToEventEnvelope());
            return newMachineJobRequest;
        }

        protected override void When(EventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Type)
            {
                case nameof(NewMachineJobRequested):
                    var newMachineJobRequested = eventEnvelope.DeserializeEvent<NewMachineJobRequested>();
                    SetIdentity(StreamId.AssembleFor<NewMachineJobRequest>(
                        newMachineJobRequested.FactoryId,
                        newMachineJobRequested.MachineId,
                        newMachineJobRequested.RequestedJobId));
                    break;
            }
        }
    }
}