using System;
using Abstractions;
using Shared;

namespace DetectMachineStop
{
    public sealed class DetectMachineStopCommand : ICommand
    {
        public string FactoryId { get; }
        public string RemoteId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }

        public DetectMachineStopCommand(
            string factoryId,
            string remoteId,
            string machineId,
            DateTime stoppedAt)
        {
            FactoryId = factoryId;
            RemoteId = remoteId;
            MachineId = machineId;
            StoppedAt = stoppedAt;
        }
        
        internal MachineStopped ToMachineStopped() => new MachineStopped(
            FactoryId,
            RemoteId,
            MachineId,
            StoppedAt);
    }
}