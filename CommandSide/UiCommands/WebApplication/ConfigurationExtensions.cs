using Microsoft.Extensions.Configuration;

namespace WebApplication
{
    public static class ConfigurationExtensions
    {
        public static string EventStoreConnectionString(this IConfiguration configuration) =>
            configuration["EventStore:ConnectionString"];
    }
}