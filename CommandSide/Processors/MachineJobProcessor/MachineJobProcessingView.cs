using System;
using Shared;

namespace MachineJobProcessor
{
    internal sealed class MachineJobProcessingView : IView
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime? MachineStartedTime { get; }
        public string JobId { get; }
        public DateTime? RequestedJobTime { get; }
        public string LastAppliedEventType { get; }

        public MachineJobProcessingView(
            string factoryId,
            string machineId,
            DateTime? machineStartedTime,
            string jobId,
            DateTime? requestedJobTime,
            string lastAppliedEventType)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            MachineStartedTime = machineStartedTime;
            JobId = jobId;
            RequestedJobTime = requestedJobTime;
            LastAppliedEventType = lastAppliedEventType;
        }

        public override string ToString()
        {
            return $"{nameof(FactoryId)}: {FactoryId}, {nameof(MachineId)}: {MachineId}, {nameof(MachineStartedTime)}: {MachineStartedTime}, {nameof(JobId)}: {JobId}, {nameof(RequestedJobTime)}: {RequestedJobTime}, {nameof(LastAppliedEventType)}: {LastAppliedEventType}";
        }
    }
}