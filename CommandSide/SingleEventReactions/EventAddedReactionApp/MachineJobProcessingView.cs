using Shared;

namespace EventAddedReactionApp
{
    internal sealed class MachineJobProcessingView : IView
    {
        public MachineState? MachineState { get; }
        public string JobId { get; }
        public JobState? JobState { get; }
        public string LastAppliedEventType { get; }

        public MachineJobProcessingView(
            MachineState? machineState,
            string jobId,
            JobState? jobState,
            string lastAppliedEventType)
        {
            MachineState = machineState;
            JobId = jobId;
            JobState = jobState;
            LastAppliedEventType = lastAppliedEventType;
        }

        public override string ToString()
        {
            return $"{nameof(MachineState)}: {MachineState}, {nameof(JobId)}: {JobId}, {nameof(JobState)}: {JobState}, {nameof(LastAppliedEventType)}: {LastAppliedEventType}";
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