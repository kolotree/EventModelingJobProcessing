﻿using System;
using JobProcessing.Abstractions;

namespace Processor.Domain
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class MachineJobProcessorView : IView
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime? MachineStartedTime { get; }
        public string JobId { get; }
        public DateTime? RequestedJobTime { get; }
        public string LastAppliedEventType { get; }
        public string LastAppliedEventId { get; }
        public string LastAppliedEventCorrelationId { get; }

        public MachineJobProcessorView(
            string factoryId,
            string machineId,
            DateTime? machineStartedTime,
            string jobId,
            DateTime? requestedJobTime,
            string lastAppliedEventType, 
            string lastAppliedEventId,
            string lastAppliedEventCorrelationId)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            MachineStartedTime = machineStartedTime;
            JobId = jobId;
            RequestedJobTime = requestedJobTime;
            LastAppliedEventType = lastAppliedEventType;
            LastAppliedEventId = lastAppliedEventId;
            LastAppliedEventCorrelationId = lastAppliedEventCorrelationId;
        }
        
        public NewMachineJobStarted? ToOptionalNewMachineJobStartedUsing(Guid newJobGuid)
        {
            if (MachineStartedTime.HasValue &&
                string.IsNullOrWhiteSpace(JobId))
            {
                return new NewMachineJobStarted(
                    FactoryId,
                    MachineId,
                    newJobGuid.ToString("N"),
                    MachineStartedTime.Value);
            }

            if (!MachineStartedTime.HasValue &&
                RequestedJobTime.HasValue)
            {
                return new NewMachineJobStarted(
                    FactoryId,
                    MachineId,
                    newJobGuid.ToString("N"),
                    RequestedJobTime.Value);
            }
            
            return null;
        }

        public override string ToString()
        {
            return $"{nameof(FactoryId)}: {FactoryId}, {nameof(MachineId)}: {MachineId}, {nameof(MachineStartedTime)}: {MachineStartedTime}, {nameof(JobId)}: {JobId}, {nameof(RequestedJobTime)}: {RequestedJobTime}, {nameof(LastAppliedEventType)}: {LastAppliedEventType}, {nameof(LastAppliedEventId)}: {LastAppliedEventId}, {nameof(LastAppliedEventCorrelationId)}: {LastAppliedEventCorrelationId}";
        }
    }
}