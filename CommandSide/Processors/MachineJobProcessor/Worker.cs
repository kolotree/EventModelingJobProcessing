using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.EventStore;
using MachineJobProcessor.Domain;
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
            _logger.LogStartupInformation(_configuration);
            
            try
            {
                await ProcessSubscription(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"[{nameof(MachineJobProcessor)}] Unhandled exception.");
                Environment.Exit(-1);
            }
        }

        private async Task ProcessSubscription(CancellationToken stoppingToken)
        {
            using var storeBuilder = EventStoreBuilder.NewUsing(_configuration.EventStoreConnectionString());
            var machineJobProcessorViewObserver = new MachineJobProcessorViewObserver(storeBuilder.NewStore());
            var persistedSubscriptionSource = storeBuilder.NewPersistedSubscriptionSource();
            
            await persistedSubscriptionSource.SubscribeTo<MachineJobProcessorView>(
                _configuration.SubscriptionRequest(),
                HandleSingleViewChange,
                stoppingToken);
            
            async Task HandleSingleViewChange(MachineJobProcessorView view)
            {
                _logger.LogDebug($"[{nameof(MachineJobProcessor)}] Received new view: {view}.");
                await machineJobProcessorViewObserver.ObserveChange(view);
                _logger.LogDebug($"[{nameof(MachineJobProcessor)}] View successfully handled: {view}.");
            }
        }
    }
}