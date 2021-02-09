using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class MachineStoppage : Stream
    {
        public static MachineStoppage NewOf(Command c)
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
                    var machineStopped = eventEnvelope.Deserialize<MachineStopped>();
                    SetIdentity(StreamId.AssembleFor<MachineStoppage>(
                        machineStopped.FactoryId,
                        machineStopped.MachineId,
                        machineStopped.StoppedAt.Ticks.ToString()));
                    break;
            }
        }
    }
}