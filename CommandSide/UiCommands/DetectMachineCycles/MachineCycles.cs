using System.Collections.Generic;
using Abstractions;
using Infrastructure.Serialization;

namespace DetectMachineCycles
{
    internal sealed class MachineCycles : IStream
    {
        private readonly DetectMachineCyclesCommand _detectMachineCycleCommand;

        private MachineCycles(DetectMachineCyclesCommand detectMachineCycleCommand)
        {
            _detectMachineCycleCommand = detectMachineCycleCommand;
        }

        public static MachineCycles From(DetectMachineCyclesCommand c) => new(c);

        public StreamId StreamId => _detectMachineCycleCommand.MachineCyclesGlobalStream;

        public IReadOnlyList<EventEnvelope> UncommittedEventEnvelopes => new List<EventEnvelope>
        {
            _detectMachineCycleCommand.ToMachineCycleDetected().ToEventEnvelope()
        };
    }
}