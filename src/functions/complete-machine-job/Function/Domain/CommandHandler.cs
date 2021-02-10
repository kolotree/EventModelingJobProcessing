using System.Threading.Tasks;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    internal sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly IStore _store;

        public CommandHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task Handle(Command c)
        {
            var machineJob = await _store.Get<MachineJob>(c.JobStream);
            machineJob.Execute(c);
            await _store.SaveChanges(machineJob);
        }
    }
}