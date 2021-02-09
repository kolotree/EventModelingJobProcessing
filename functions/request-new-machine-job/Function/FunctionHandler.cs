using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using JobProcessing.Abstractions;
using Function.Domain;
using JobProcessing.Infrastructure;

namespace Function
{
    public sealed class FunctionHandler
    {
        private readonly IStore _store;
        private readonly IDateTimeProvider _dateTimeProvider;

        public FunctionHandler(IStore store, IDateTimeProvider? dateTimeProvider = null)
        {
            _store = store;
            _dateTimeProvider = dateTimeProvider ?? new StandardDateTimeProvider();
        }
        
        public async Task<FunctionResult> Handle(HttpRequest request)
        {
            try
            {
                var command = await request.Read<Command>();
                
                var commandHandler = new CommandHandler(_store, _dateTimeProvider);
                await commandHandler.Handle(command);
                
                return FunctionResult.SuccessWith(command.JobStartTime.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case VersionMismatchException: return FunctionResult.BadRequestFailureWith("Item with the same ID already in store");
                    case ArgumentException e: return FunctionResult.BadRequestFailureWith($"Invalid input: {e.ParamName}");
                    default: throw;
                }
            }
        }
    }
}