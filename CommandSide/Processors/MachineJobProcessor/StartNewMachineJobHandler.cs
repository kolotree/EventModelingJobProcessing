using System.Threading.Tasks;
using Abstractions;

namespace MachineJobProcessor
{
    public sealed class StartNewMachineJobHandler : ICommandHandler<StartNewMachineJobCommand>
    {
        private readonly IStore _store;

        public StartNewMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(StartNewMachineJobCommand c)
        {
            return _store.SaveChanges(MachineJob.NewStartedJobFrom(c));
        }
    }
}