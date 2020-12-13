using System;
using Shared;

namespace MachineJobProcessor
{
    internal sealed class MachineJobProcessingView : IView
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public MachineState? MachineState { get; }
        public DateTime? MachineStartedTime { get; }
        public string JobId { get; }
        public JobState? JobState { get; }
        public string LastAppliedEventType { get; }

        public MachineJobProcessingView(
            string factoryId,
            string machineId,
            MachineState? machineState,
            DateTime? machineStartedTime,
            string jobId,
            JobState? jobState,
            string lastAppliedEventType)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            MachineState = machineState;
            MachineStartedTime = machineStartedTime;
            JobId = jobId;
            JobState = jobState;
            LastAppliedEventType = lastAppliedEventType;
        }
    }
    
    internal enum MachineState
    {
        Stopped,
        Started
    }

    internal enum JobState
    {
        Started,
        Completed
    }
}