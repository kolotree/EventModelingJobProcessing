using System;
using Abstractions;

namespace Function.Domain
{
    public sealed class RequestNewMachineJob : ICommand
    {
        public string FactoryId { get; }

        public string MachineId { get; }

        public RequestNewMachineJob(string factoryId, string machineId)
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