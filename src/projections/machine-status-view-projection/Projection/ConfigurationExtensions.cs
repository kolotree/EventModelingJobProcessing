using System;
using System.Linq;
using EventStore.Client;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.EventStore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using Projection.Domain;
using ViewStore.Abstractions;
using ViewStore.MongoDb;

// ReSharper disable NotResolvedInText

namespace Projection
{
    internal static class ConfigurationExtensions
    {
        public static IClientSubscriptionSource ClientSubscriptionSource(this IConfiguration configuration) =>
            EventStoreBuilder
                .NewUsing(configuration.EventStoreConfiguration())
                .NewClientSubscriptionSource();
        
        public static IViewStore MongoDbViewStore(this IConfiguration configuration) =>
            MongoDbViewStoreBuilder.New()
                .WithConnectionDetails(
                    configuration.MongoDb().ConnectionString,
                    configuration.MongoDb().DatabaseName)
                .WithCollectionName(configuration.MachineStatusViewModel())
                .UseViewRegistrator(() =>
                {
                    BsonClassMap.RegisterClassMap<MachineStatusView>();
                })
                .Build();
        
        private static EventStoreConfiguration EventStoreConfiguration(this IConfiguration configuration) =>
            new(
                configuration["EventStore:ConnectionString"] ?? throw new ArgumentNullException("EventStore:ConnectionString"),
                new UserCredentials(
                    configuration["EventStore:Credentials"]?.Split(':').FirstOrDefault() ?? throw new ArgumentNullException("EventStore:Credentials"),
                    configuration["EventStore:Credentials"]?.Split(':').LastOrDefault() ?? throw new ArgumentNullException("EventStore:Credentials")));

        private static MongoDbConfiguration MongoDb(this IConfiguration configuration) =>
            new(
                configuration["MongoDb:ConnectionString"] ?? throw new ArgumentNullException("MongoDb:ConnectionString"),
                configuration["MongoDb:DatabaseName"] ?? throw new ArgumentNullException("MongoDb:DatabaseName"));

        private static string MachineStatusViewModel(this IConfiguration configuration) =>
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