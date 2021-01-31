using Abstractions;
using Infrastructure.Serialization;

namespace DetectMachineStop
{
    internal sealed class MachineStoppage : Stream
    {
        public static MachineStoppage NewOf(DetectMachineStopCommand c)
        {
            var machineStoppage = new MachineStoppage();
            machineStoppage.ApplyChange(c.ToMachineStopped().ToEventEnvelope());
            return machineStoppage;
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
                    break;
            }
        }
    }
}