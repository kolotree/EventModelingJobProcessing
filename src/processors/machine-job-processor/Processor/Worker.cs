using System;
using System.Threading;
using System.Threading.Tasks;
using JobProcessing.Infrastructure.EventStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Processor.Domain;

namespace Processor
{
    internal class Worker : BackgroundService
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
                _logger.LogCritical(e, "Unhandled exception occurred while handling subscription");
                Environment.Exit(-1);
            }
        }

        private async Task ProcessSubscription(CancellationToken stoppingToken)
        {
            using var storeBuilder = EventStoreBuilder.NewUsing(_configuration.EventStoreConfiguration());
            var machineJobProcessorViewObserver = new MachineJobProcessorViewObserver(storeBuilder.NewStore());
            var persistedSubscriptionSource = storeBuilder.NewPersistedSubscriptionSource();
            
            await persistedSubscriptionSource.SubscribeTo<MachineJobProcessorView>(
                _configuration.SubscriptionRequest(),
                HandleSingleViewChange,
                stoppingToken);
            
            async Task HandleSingleViewChange(MachineJobProcessorView view)
            {
                _logger.LogDebug("Received new view: {View}", view);
                await machineJobProcessorViewObserver.ObserveChange(view);
                _logger.LogDebug("View successfully handled: {View}", view);
            }
        }
    }
}