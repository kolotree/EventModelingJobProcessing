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
            var stoppage = await _store.Get<MachineStoppage>(c.StoppageId);
            stoppage.Apply(c);
            await _store.SaveChanges(stoppage);
        }
    }
}