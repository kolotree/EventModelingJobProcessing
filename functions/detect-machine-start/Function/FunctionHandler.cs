using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using JobProcessing.Abstractions;
using Function.Domain;

namespace Function
{
    public sealed class FunctionHandler
    {
        private readonly IStore _store;

        public FunctionHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task<FunctionResult> Handle(HttpRequest request)
        {
            try
            {
                var command = await request.Read<Command>();
                var commandHandler = new CommandHandler(_store);
                await commandHandler.Handle(command);
                return FunctionResult.Success;
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case StreamDoesntExist: return FunctionResult.BadRequestFailureWith("Stoppage doesn't exist.");
                    case ArgumentException e: return FunctionResult.BadRequestFailureWith($"Invalid input: {e.ParamName}");
                    default: throw;
                }
            }
        }
    }
}