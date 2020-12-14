﻿using Abstractions;
using Shared;

namespace CompleteMachineStoppage
{
    internal sealed class MachineJob : AggregateRoot
    {
        private bool _isCompleted;
        
        public void Execute(CompleteMachineJobCommand c)
        {
            if (!_isCompleted)
            {
                ApplyChange(c.ToMachineJobComplete());
            }
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
                    _isCompleted = false;
                    break;
                case  MachineJobCompleted machineJobComplete:
                    _isCompleted = true;
                    break;
            }
        }
    }
}