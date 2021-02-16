using System;
using System.Linq;
using EventStore.Client;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.EventStore;
using Microsoft.Extensions.Configuration;

namespace Processor
{
    internal static class ConfigurationExtensions
    {
        public static EventStoreConfiguration EventStoreConfiguration(this IConfiguration configuration) =>
            new(
                configuration["EventStore:ConnectionString"] ?? throw new ArgumentNullException("EventStore:ConnectionString"),
                new UserCredentials(
                    configuration["EventStore:Credentials"]?.Split(':').FirstOrDefault() ?? throw new ArgumentNullException("EventStore:Credentials"),
                    configuration["EventStore:Credentials"]?.Split(':').LastOrDefault() ?? throw new ArgumentNullException("EventStore:Credentials")));

        public static SubscriptionRequest SubscriptionRequest(this IConfiguration configuration) =>
            new(
                configuration["StreamName"],
                configuration["SubscriptionGroupName"],
                long.Parse(configuration["ProjectStartingFromEventPosition"]));
    }
}