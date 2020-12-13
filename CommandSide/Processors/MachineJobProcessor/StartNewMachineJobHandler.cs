using System.Threading.Tasks;
using Abstractions;

namespace MachineJobProcessor
{
    internal sealed class StartNewMachineJobHandler : ICommandHandler<StartNewMachineJobCommand>
    {
        private readonly IStore _store;

        public StartNewMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(StartNewMachineJobCommand c) =>
            MachineJob.NewStartedJobFrom(c)
                .Map(machineJob => _store.SaveChanges(machineJob))
                .Unwrap(Task.CompletedTask);
    }
}