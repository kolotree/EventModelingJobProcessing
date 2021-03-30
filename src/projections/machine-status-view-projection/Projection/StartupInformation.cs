using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Projection.Domain;

namespace Projection
{
    internal static class StartupInformation
    {
        private static string Version = "0.0.1";
        
        public static void LogStartupInformation(this ILogger logger, IConfiguration configuration)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{nameof(MachineStatusView)}] Starting worker ...");
            stringBuilder.AppendLine($"[{nameof(MachineStatusView)}] Version: {Version}");
            
            logger.LogInformation("{StartupInformation}", stringBuilder);
        }
    }
}