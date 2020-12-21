using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommandsWireUp;
using DetectMachineCycles;
using Infrastructure.EventStore;

namespace MachineCyclesTestApp
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
                    var happenedAt = DateTime.UtcNow;
                    await externalCommandHandlers.Handle(new DetectMachineCyclesCommand(
                        factoryId,
                        machineId,
                        new List<DateTime> {happenedAt}));
                }));
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds / 1000m);
        }
    }
}