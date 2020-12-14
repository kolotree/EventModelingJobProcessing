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
        public string LastAppliedEventType { get; }

        public MachineJobProcessingView(
            string factoryId,
            string machineId,
            DateTime? machineStartedTime,
            string jobId,
            string lastAppliedEventType)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            MachineStartedTime = machineStartedTime;
            JobId = jobId;
            LastAppliedEventType = lastAppliedEventType;
        }
    }
}