using System.Threading.Tasks;
using Abstractions;

namespace MachineJobProcessor.Domain
{
    internal sealed class StartNewMachineJobHandler : ICommandHandler<StartNewMachineJobCommand>
    {
        private readonly IStore _store;

        public StartNewMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(StartNewMachineJobCommand c) =>
            MachineJob.OptionalNewStartedJobFrom(c)
                .Map(machineJob => _store.SaveChanges(machineJob))
                .Unwrap(Task.CompletedTask);
    }
}