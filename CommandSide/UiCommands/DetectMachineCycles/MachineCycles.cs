using System.Collections.Generic;
using Abstractions;
using Shared;

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

        public IReadOnlyList<IEvent> UncommittedEvents => new List<IEvent>
        {
            _detectMachineCycleCommand.ToMachineCycleDetected()
        };
    }
}