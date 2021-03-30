using ViewStore.Abstractions;

namespace Processor.Domain
{
    internal sealed class MachineStatusView : IView
    {
        public long NumberOfStops { get; private set; }
        public long NumberOfStarts { get; private set; }

        public MachineStatusView(
            long numberOfStops,
            long numberOfStarts)
        {
            NumberOfStops = numberOfStops;
            NumberOfStarts = numberOfStarts;
        }

        public static MachineStatusView New()
            => new(0, 0);

        public void Apply(MachineStopped _)
        {
            NumberOfStops++;
        }
        
        public void Apply(MachineStarted _)
        {
            NumberOfStarts++;
        }

        public override string ToString()
        {
            return $"{nameof(NumberOfStops)}: {NumberOfStops}, {nameof(NumberOfStarts)}: {NumberOfStarts}";
        }
    }
}