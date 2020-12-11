using System;
using System.Threading.Tasks;
using Infrastructure.EventStore;
using Shared;

namespace EventAddedReactionApp
{
    class Program
    {
        static async Task Main()
        {
            using var storeBuilder = EventStoreBuilder.NewUsing("tcp://admin:changeit@localhost:1113");

            await storeBuilder.NewPersistedEventSource().SubscribeTo<MachineStopped>(
                machineStopped =>
                {
                    Console.WriteLine($"New Machine Stoppage detected at '{machineStopped.StoppedAt}' for machine {machineStopped.MachineId}.");
                    return Task.CompletedTask;
                });
        }
    }
}