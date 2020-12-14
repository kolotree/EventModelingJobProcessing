using System;
using System.Threading.Tasks;
using Abstractions;

namespace DetectMachineStop
{
    public sealed class DetectMachineStopHandler : ICommandHandler<DetectMachineStopCommand>
    {
        private readonly IStore _store;

        public DetectMachineStopHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(DetectMachineStopCommand c)
        {
            return _store.SaveChanges(MachineStoppage.NewOf(c));
        }
    }
}