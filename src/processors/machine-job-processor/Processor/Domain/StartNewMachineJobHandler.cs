using System.Threading.Tasks;
using JobProcessing.Abstractions;

namespace MachineJobProcessor.Domain
{
    internal sealed class StartNewMachineJobHandler : ICommandHandler<StartNewMachineJobCommand>
    {
        private readonly IStore _store;

        public StartNewMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(StartNewMachineJobCommand c)
        {
            var optionalMachineJob = MachineJob.OptionalNewStartedJobFrom(c);
            if (optionalMachineJob != null)
            {
                return _store.SaveChanges(optionalMachineJob);
            }

            return Task.CompletedTask;
        }
    }
}