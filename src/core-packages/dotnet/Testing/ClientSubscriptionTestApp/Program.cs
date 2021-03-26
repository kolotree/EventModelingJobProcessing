using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.EventStore;

namespace ClientSubscriptionTestApp
{
    class Program
    {
        static async Task Main()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var subscriptionTask = EventStoreBuilder
                .NewUsing(new EventStoreConfiguration(
                    "esdb://culaja.com:2111,culaja.com:2112,culaja.com:2113?tls=true&tlsVerifyCert=false",
                    new UserCredentials("admin", "changeit")))
                .NewClientSubscriptionSource()
                .SubscribeUsing(
                    new ClientSubscriptionRequest(GlobalPosition.Of(24970), "MachineStopped", "MachineStarted"),
                    (eventEnvelope, globalPosition) =>
                    {
                        Console.WriteLine($"{globalPosition}:{eventEnvelope.Data}, {eventEnvelope.Metadata}");
                        return Task.CompletedTask;
                    },
                    cancellationTokenSource.Token);

            Console.ReadLine();
            
            cancellationTokenSource.Cancel();
            await subscriptionTask;
        }
    }
}