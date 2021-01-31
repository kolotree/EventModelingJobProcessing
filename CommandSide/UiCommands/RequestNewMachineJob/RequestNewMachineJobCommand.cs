using System;
using Abstractions;

namespace RequestNewMachineJob
{
    public sealed class RequestNewMachineJobCommand : ICommand
    {
        public string FactoryId { get; }

        public string MachineId { get; }

        public RequestNewMachineJobCommand(string factoryId, string machineId)
        {
            FactoryId = factoryId;
            MachineId = machineId;
        }

        public NewMachineJobRequested ToNewMachineJobRequestedUsing(DateTime jobStartTime) => new(
            FactoryId,
            MachineId,
            jobStartTime.Ticks.ToString(),
            jobStartTime);
    }
}