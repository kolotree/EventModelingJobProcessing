using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommandsWireUp;
using DetectMachineStart;
using DetectMachineStop;
using Infrastructure.EventStore;

namespace MachineStoppagesTestApp
{
    class Program
    {
        public static async Task Main()
        {
            var eventStoreBuilder = EventStoreBuilder.NewUsing("tcp://admin:changeit@localhost:1113");
            var externalCommandHandlers = ExternalCommandHandlers.NewWith(eventStoreBuilder.NewStore());

            await ExecuteSingleBatch(externalCommandHandlers, 1000);
        }

        private static async Task ExecuteSingleBatch(
            ExternalCommandHandlers externalCommandHandlers,
            int count)
        {
            var sw = new Stopwatch();
            sw.Start();
            await Task.WhenAll(Enumerable.Range(0, count)
                .Select(async i =>
                {
                    var factoryId = Guid.NewGuid().ToString("N");
                    var machineId = i.ToString();
                    var stoppedAt = DateTime.UtcNow;
                    var startedAt = DateTime.UtcNow;
                    await externalCommandHandlers.Handle(new DetectMachineStopCommand(factoryId, machineId, stoppedAt));
                    await externalCommandHandlers.Handle(new DetectMachineStartCommand(factoryId, machineId, stoppedAt,
                        startedAt));
                }));
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds / 1000m);
        }
    }
}