using System.IO;
using System.Threading.Tasks;
using JobProcessing.Abstractions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Function
{
    internal static class HttpRequestExtensions
    {
        public static async Task<T> Read<T>(this HttpRequest httpRequest) where T : ICommand
        {
            var reader = new StreamReader(httpRequest.Body);
            var input = await reader.ReadToEndAsync();
            var command = JsonConvert.DeserializeObject<T>(input);
            return command;
        }
    }
}