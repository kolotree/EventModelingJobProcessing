using System;
using CompleteMachineStoppage;
using DetectMachineStart;
using DetectMachineStop;
using RequestNewMachineJob;

namespace WebApplication.Controllers
{
    public sealed class DetectMachineStopDto
    {
        public string? FactoryId { get; set; }
        public string? MachineId { get; set; }
        public DateTime StoppedAt { get; set; }
        
        internal DetectMachineStopCommand ToCommand() => new(
            FactoryId.TryUnwrap(nameof(FactoryId)),
            MachineId.TryUnwrap(nameof(MachineId)),
            StoppedAt);
    }
    
    public sealed class DetectMachineStartDto
    {
        public string? FactoryId { get; set;}
        public string? MachineId { get; set;}
        public DateTime LastStoppedAt  { get; set;}
        public DateTime StartedAt { get; set;}
        
        internal DetectMachineStartCommand ToCommand() => new(
            FactoryId.TryUnwrap(nameof(FactoryId)),
            MachineId.TryUnwrap(nameof(MachineId)),
            LastStoppedAt,
            StartedAt);
    }
    
    public sealed class CompleteMachineJobDto
    {
        public string? FactoryId { get; set;}
        public string? MachineId { get; set;}
        public string? JobId  { get; set;}
        
        internal CompleteMachineJobCommand ToCommand() => new(
            FactoryId.TryUnwrap(nameof(FactoryId)),
            MachineId.TryUnwrap(nameof(MachineId)),
            JobId.TryUnwrap(nameof(JobId)));
    }
    
    public sealed class RequestNewMachineJobDto
    {
        public string? FactoryId { get; set;}
        public string? MachineId { get; set;}
        
        internal RequestNewMachineJobCommand ToCommand() => new(
            FactoryId.TryUnwrap(nameof(FactoryId)),
            MachineId.TryUnwrap(nameof(MachineId)));
    }
}