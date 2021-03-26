using System.Text;
using JobProcessing.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Processor.Domain;

namespace Processor
{
    internal static class StartupInformation
    {
        private static string Version = "0.0.1";
        
        public static void LogStartupInformation(this ILogger logger, IConfiguration configuration)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{nameof(MachineStatusView)}] Starting worker ...");
            stringBuilder.AppendLine($"[{nameof(MachineStatusView)}] Version: {Version}");
            
            logger.LogInformation(stringBuilder.ToString());
        }
    }
}