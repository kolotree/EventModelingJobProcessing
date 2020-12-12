using System.Collections.Generic;

namespace Shared
{
    public sealed class MachineCreated : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string RemoteId { get; }
        public string MachineId { get; }

        public MachineCreated(
            string factoryId,
            string remoteId,
            string machineId)
        {
            FactoryId = factoryId;
            RemoteId = remoteId;
            MachineId = machineId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return RemoteId;
            yield return MachineId;
        }
    }
}