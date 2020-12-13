using System;
using Abstractions;
using Shared;

namespace DetectMachineStart
{
    public sealed class DetectMachineStartCommand : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime LastStoppedAt  { get; }
        public DateTime StartedAt { get; }

        public DetectMachineStartCommand(
            string factoryId,
            string machineId,
            DateTime lastStoppedAt,
            DateTime startedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            LastStoppedAt = lastStoppedAt;
            StartedAt = startedAt;
        }
        
        internal StreamId StoppageId => StreamId.AssembleFor<MachineStoppage>(
            FactoryId,
            MachineId,
            LastStoppedAt.Ticks.ToString());
        
        internal MachineStarted ToMachineStarted() => new MachineStarted(
            FactoryId,
            MachineId,
            LastStoppedAt,
            StartedAt);
    }
}