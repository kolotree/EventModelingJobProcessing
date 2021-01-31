using System;
using Abstractions;

namespace DetectMachineStop
{
    public sealed class DetectMachineStopCommand : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }

        public DetectMachineStopCommand(
            string factoryId,
            string machineId,
            DateTime stoppedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            StoppedAt = stoppedAt;
        }
        
        internal MachineStopped ToMachineStopped() => new(
            FactoryId,
            MachineId,
            StoppedAt);
    }
}