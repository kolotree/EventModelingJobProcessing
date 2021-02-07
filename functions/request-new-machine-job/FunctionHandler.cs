using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Abstractions;
using Function.Domain;
using Infrastructure.EventStore;
using Newtonsoft.Json;

namespace Function
{
    public sealed class FunctionHandler
    {
        private readonly IStore _store;

        public FunctionHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task<(int, string)> Handle(HttpRequest request)
        {
            var command = await request.Read<RequestNewMachineJob>();
            var commandHandler = new RequestNewMachineJobHandler(_store);
            await commandHandler.Handle(command);
            return (200, "{}");
        }
    }
}