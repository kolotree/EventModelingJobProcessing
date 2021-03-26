using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using Processor.Domain;

namespace Processor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BsonClassMap.RegisterClassMap<MachineStatusView>();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) => { services.AddHostedService<Worker>(); });
    }
}