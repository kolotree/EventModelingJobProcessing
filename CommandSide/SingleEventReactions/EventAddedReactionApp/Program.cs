using System;
using System.Threading.Tasks;
using Infrastructure.EventStore;

namespace EventAddedReactionApp
{
    class Program
    {
        static async Task Main()
        {
            using var storeBuilder = EventStoreBuilder.NewUsing("tcp://admin:changeit@localhost:1113");

            await storeBuilder.NewPersistedEventSource().SubscribeToView<MachineJobProcessingView>(
                view =>
                {
                    Console.WriteLine($"New {nameof(MachineJobProcessingView)} received: {view}");
                    return Task.CompletedTask;
                });
        }
    }
}