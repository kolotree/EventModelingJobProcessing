using System;
using System.Threading;
using System.Threading.Tasks;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Processor.Domain;
using ViewStore.Abstractions;

// ReSharper disable MethodHasAsyncOverload

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
            var clientSubscriptionSource = _configuration.ClientSubscriptionSource();
            var viewStore = _configuration.MongoDbViewStore();

            async Task TransformView(string viewId, GlobalPosition globalPosition, Action<MachineStatusView> transform)
            {
                var machineStatusViewEnvelope = await viewStore!.ReadAsync(viewId)
                                                ?? new ViewEnvelope(viewId, MachineStatusView.New(), GlobalVersion.Start);
                
                machineStatusViewEnvelope.Transform(globalPosition.ToGlobalVersion(), transform);
                
                await viewStore.SaveAsync(machineStatusViewEnvelope);
            }

            await clientSubscriptionSource.SubscribeUsing(
                new ClientSubscriptionRequest(
                    viewStore.ReadLastGlobalVersion()?.ToGlobalPosition() ?? GlobalPosition.Start,
                    nameof(MachineStopped), nameof(MachineStarted)), async (eventEnvelope, globalPosition) =>
                {
                    Console.WriteLine(eventEnvelope.Data);
                    switch (eventEnvelope.Type)
                    {
                        case nameof(MachineStopped):
                            var machineStopped = eventEnvelope.Deserialize<MachineStopped>();
                            await TransformView(machineStopped.ViewId, globalPosition, view => view.Apply(machineStopped));
                            break;
                        case nameof(MachineStarted):
                            var machineStarted = eventEnvelope.Deserialize<MachineStarted>();
                            await TransformView(machineStarted.ViewId, globalPosition, view => view.Apply(machineStarted));
                            break;
                    }
                },
                stoppingToken);
        }
    }
}