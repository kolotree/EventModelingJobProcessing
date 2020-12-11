using System;
using DetectMachineStop;

namespace WebApplication.Controllers
{
    public sealed class DetectMachineStopDto
    {
        public string FactoryId { get; set; }
        public string RemoteId { get; set; }
        public string MachineId { get; set; }
        public DateTime StoppedAt { get; set; }
        
        internal DetectMachineStopCommand ToCommand() => new DetectMachineStopCommand(
            FactoryId,
            RemoteId,
            MachineId,
            StoppedAt);
    }
}