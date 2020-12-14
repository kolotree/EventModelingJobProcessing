using Abstractions;
using Microsoft.Extensions.Configuration;

namespace MachineJobProcessor
{
    internal static class ConfigurationExtensions
    {
        public static string EventStoreConnectionString(this IConfiguration configuration) =>
            configuration["EventStore:ConnectionString"];

        public static SubscriptionRequest SubscriptionRequest(this IConfiguration configuration) =>
            new SubscriptionRequest(
                configuration["StreamName"],
                configuration["SubscriptionGroupName"],
                long.Parse(configuration["ProjectStartingFromEventPosition"]));
    }
}