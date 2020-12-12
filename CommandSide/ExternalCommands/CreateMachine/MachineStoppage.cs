using Abstractions;
using Shared;

namespace CreateMachine
{
    internal sealed class Machine : AggregateRoot
    {
        public static Machine NewOf(CreateMachineCommand c)
        {
            var machine = new Machine();
            machine.ApplyChange(c.ToMachineCreated());
            return machine;
        }
        
        protected override void When(IEvent e)
        {
            switch (e)
            {
                case MachineCreated machineStopped:
                    SetIdentity(StreamId.AssembleFor<Machine>(
                        machineStopped.FactoryId,
                        machineStopped.RemoteId,
                        machineStopped.MachineId));
                    break;
            }
        }
    }
}