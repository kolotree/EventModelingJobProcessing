using System;
using Abstractions;
using Shared;

namespace MachineJobProcessor
{
    public sealed class StartNewMachineJobCommand : ICommand
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime StartedAt { get; }

        public StartNewMachineJobCommand(
            string factoryId,
            string machineId,
            DateTime startedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            StartedAt = startedAt;
        }
        
        public NewMachineJobStarted ToNewMachineJobStarted() =>
            new NewMachineJobStarted(
                FactoryId,
                MachineId,
                StartedAt.Ticks.ToString(),
                StartedAt);
    }
}