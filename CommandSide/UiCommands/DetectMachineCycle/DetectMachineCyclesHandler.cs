using System.Threading.Tasks;
using Abstractions;

namespace DetectMachineCycle
{
    public sealed class DetectMachineCyclesHandler : ICommandHandler<DetectMachineCyclesCommand>
    {
        private readonly IStore _store;

        public DetectMachineCyclesHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(DetectMachineCyclesCommand c) => 
            _store.AppendTo(MachineCycles.From(c));
    }
}