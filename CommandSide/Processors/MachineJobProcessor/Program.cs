using System.Threading.Tasks;
using Infrastructure.EventStore;
using Shared;
// ReSharper disable PossibleInvalidOperationException

namespace MachineJobProcessor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var storeBuilder = EventStoreBuilder.NewUsing("tcp://admin:changeit@localhost:1113");

            var store = storeBuilder.NewStore();
            await storeBuilder.NewPersistedSubscriptionSource().SubscribeToView<MachineJobProcessingView>(
                view =>
                {
                    switch (view.LastAppliedEventType)
                    {
                        case nameof(MachineStarted):
                            return new StartNewMachineJobHandler(store).Handle(new StartNewMachineJobCommand(
                                view.FactoryId,
                                view.MachineId,
                                view.MachineStartedTime.Value));
                    }
                    
                    return Task.CompletedTask;
                });
        }
    }
}