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
        
        public async Task Handle(Command c)
        {
            try
            {
                await _store.SaveChanges(MachineStoppage.NewOf(c));
            }
            catch (VersionMismatchException)
            {
            }
        }
    }
}