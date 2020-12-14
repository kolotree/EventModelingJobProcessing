﻿using System;
using CompleteMachineStoppage;
using DetectMachineStart;
using DetectMachineStop;

namespace WebApplication.Controllers
{
    public sealed class DetectMachineStopDto
    {
        public string FactoryId { get; set; }
        public string MachineId { get; set; }
        public DateTime StoppedAt { get; set; }
        
        internal DetectMachineStopCommand ToCommand() => new DetectMachineStopCommand(
            FactoryId,
            MachineId,
            StoppedAt);
    }
    
    public sealed class DetectMachineStartDto
    {
        public string FactoryId { get; set;}
        public string MachineId { get; set;}
        public DateTime LastStoppedAt  { get; set;}
        public DateTime StartedAt { get; set;}
        
        internal DetectMachineStartCommand ToCommand() => new DetectMachineStartCommand(
            FactoryId,
            MachineId,
            LastStoppedAt,
            StartedAt);
    }
    
    public sealed class CompleteMachineJob
    {
        public string FactoryId { get; set;}
        public string MachineId { get; set;}
        public string JobId  { get; set;}
        
        internal CompleteMachineJobCommand ToCommand() => new CompleteMachineJobCommand(
            FactoryId,
            MachineId,
            JobId);
    }
}