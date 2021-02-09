using System.Threading.Tasks;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly IStore _store;

        public CommandHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(Command c)
        {
            return _store.SaveChanges(MachineStoppage.NewOf(c));
        }
    }
}