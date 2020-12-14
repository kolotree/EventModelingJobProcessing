using System.Threading.Tasks;
using Abstractions;

namespace CompleteMachineStoppage
{
    public sealed class CompleteMachineJobHandler : ICommandHandler<CompleteMachineJobCommand>
    {
        private readonly IStore _store;

        public CompleteMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task Handle(CompleteMachineJobCommand c)
        {
            var machineJob = await _store.Get<MachineJob>(c.JobStream);
            machineJob.Execute(c);
            await _store.SaveChanges(machineJob);
        }
    }
}