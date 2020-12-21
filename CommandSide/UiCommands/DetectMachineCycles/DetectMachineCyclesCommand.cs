using System;
using System.Collections.Generic;
using Abstractions;
using Shared;

namespace DetectMachineCycles
{
    public sealed class DetectMachineCyclesCommand : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public IReadOnlyList<DateTime> Timestamps  { get; }

        public DetectMachineCyclesCommand(
            string factoryId,
            string machineId,
            IReadOnlyList<DateTime> timestamps)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            Timestamps = timestamps;
        }
        
        internal StreamId MachineCyclesGlobalStream => StreamId.AssembleFor<MachineCycles>(
            FactoryId,
            MachineId);
        
        internal MachineCyclesDetected ToMachineCycleDetected() => new(
            FactoryId,
            MachineId,
            Timestamps);
    }
}