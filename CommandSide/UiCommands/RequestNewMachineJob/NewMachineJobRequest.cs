using System;
using Abstractions;
using Shared;

namespace RequestNewMachineJob
{
    internal sealed class NewMachineJobRequest : Stream
    {
        public static NewMachineJobRequest From(RequestNewMachineJobCommand c, DateTime jobStartTime)
        {
            var newMachineJobRequest = new NewMachineJobRequest();
            newMachineJobRequest.ApplyChange(c.ToNewMachineJobRequestedUsing(jobStartTime));
            return newMachineJobRequest;
        }

        protected override void When(IEvent e)
        {
            switch (e)
            {
                case NewMachineJobRequested newMachineJobRequested:
                    SetIdentity(StreamId.AssembleFor<NewMachineJobRequest>(
                        newMachineJobRequested.FactoryId,
                        newMachineJobRequested.MachineId,
                        newMachineJobRequested.RequestedJobId));
                    break;
            }
        }
    }
}