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

        public static MongoDbConfiguration MongoDb(this IConfiguration configuration) =>
            new MongoDbConfiguration(
                configuration["MongoDb:ConnectionString"] ?? throw new ArgumentNullException("MongoDb:ConnectionString"),
                configuration["MongoDb:DatabaseName"] ?? throw new ArgumentNullException("MongoDb:DatabaseName"));

        public static string MachineStatusViewModel(this IConfiguration configuration) =>
            configuration["MachineStatusViewModel"] ?? throw new ArgumentNullException("MachineStatusViewModel");
    }

    internal sealed class MongoDbConfiguration
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }

        public MongoDbConfiguration(
            string connectionString,
            string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }
    }
}