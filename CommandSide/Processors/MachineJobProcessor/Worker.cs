using System.Threading;
using System.Threading.Tasks;
using Infrastructure.EventStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;
using static System.Guid;

namespace MachineJobProcessor
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;

        public Worker(IConfiguration configuration, ILogger<Worker> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var storeBuilder = EventStoreBuilder.NewUsing(_configuration.EventStoreConnectionString());
            var store = storeBuilder.NewStore();
            var persistedSubscriptionSource = storeBuilder.NewPersistedSubscriptionSource();
            
            await persistedSubscriptionSource.SubscribeTo<MachineJobProcessorView>(
                _configuration.SubscriptionRequest(),
                HandleSingleViewChange,
                stoppingToken);

            async Task HandleSingleViewChange(MachineJobProcessorView view)
            {
                _logger.LogDebug($"[{nameof(MachineJobProcessor)}] Received new view: {view}.");
                switch (view.LastAppliedEventType)
                {
                    case nameof(MachineStarted):
                    case nameof(MachineJobCompleted):
                    case nameof(NewMachineJobRequested):
                        await new StartNewMachineJobHandler(store).Handle(new StartNewMachineJobCommand(view,NewGuid()));
                        _logger.LogInformation($"[{nameof(MachineJobProcessor)}] View successfully handled: {view}.");
                        break;
                }
            }
        }
    }
}