using Abstractions;
using Shared;

namespace CreateMachine
{
    public sealed class CreateMachineCommand : ICommand
    {
        public string FactoryId { get; }
        public string RemoteId { get; }
        public string MachineId { get; }

        public CreateMachineCommand(
            string factoryId,
            string remoteId,
            string machineId)
        {
            FactoryId = factoryId;
            RemoteId = remoteId;
            MachineId = machineId;
        }

        public MachineCreated ToMachineCreated() => new MachineCreated(
            FactoryId,
            RemoteId,
            MachineId);
    }
}