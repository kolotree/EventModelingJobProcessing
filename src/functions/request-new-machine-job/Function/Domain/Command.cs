using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class Command : ICommand
    {
        public CommandMetadata Metadata { get; }
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime JobStartTime { get; }

        public Command(
            CommandMetadata? metadata,
            string? factoryId,
            string? machineId,
            DateTime? jobStartTime)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(Metadata));
            FactoryId = factoryId ?? throw new ArgumentNullException(nameof(factoryId));
            MachineId = machineId ?? throw new ArgumentNullException(nameof(machineId));
            JobStartTime = jobStartTime ?? throw new ArgumentNullException(nameof(jobStartTime));
        }

        public NewMachineJobRequested ToNewMachineJobRequestedUsing(IDateTimeProvider dateTimeProvider) => 
            JobStartTime <= dateTimeProvider.CurrentUtcDateTime
                ? new NewMachineJobRequested(
                    FactoryId,
                    MachineId,
                    JobStartTime.Ticks.ToString(),
                    JobStartTime)
                : throw new JobStartTimeCantBeInFuture(JobStartTime, dateTimeProvider.CurrentUtcDateTime);
        
        internal sealed class JobStartTimeCantBeInFuture : ArgumentException
        {
            public JobStartTimeCantBeInFuture(DateTime jobStartTime, DateTime currentTime)
                : base($"Job start time can't be in the future: {jobStartTime}. (Current time: {currentTime})", nameof(JobStartTime))
            {
            }
        }
    }
}