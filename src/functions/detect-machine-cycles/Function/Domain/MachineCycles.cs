using System.Collections.Generic;
using System.Linq;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;

namespace Function.Domain
{
    internal sealed class MachineCycles : IStream
    {
        private readonly IReadOnlyList<MachineCyclesDetected> _uncommittedEvents;

        private MachineCycles(
            StreamId streamId,
            IReadOnlyList<MachineCyclesDetected> uncommittedEvents)
        {
            StreamId = streamId;
            _uncommittedEvents = uncommittedEvents;
        }

        public static MachineCycles From(Command c) => new(c.MachineCyclesGlobalStream, c.ToMachineCycleDetectedList());

        public StreamId StreamId { get; }

        public IReadOnlyList<EventEnvelope> UncommittedEventEnvelopes => 
            _uncommittedEvents.Select(e => e.ToEventEnvelope()).ToList();
    }
}