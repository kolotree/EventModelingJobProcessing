using System.Threading.Tasks;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly IStore _store;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandHandler(IStore store, IDateTimeProvider dateTimeProvider)
        {
            _store = store;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public Task Handle(Command c)
        {
            return _store.SaveChanges(NewMachineJobRequest.From(c, _dateTimeProvider));
        }
    }
}