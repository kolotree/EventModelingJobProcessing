using System.Threading.Tasks;
using Abstractions;

namespace DetectMachineStart
{
    public sealed class DetectMachineStartHandler : ICommandHandler<DetectMachineStartCommand>
    {
        private readonly IStore _store;

        public DetectMachineStartHandler(IStore store)
        {
            _store = store;
        }
        
        public async Task Handle(DetectMachineStartCommand c)
        {
            var stoppage = await _store.Get<MachineStoppage>(c.StoppageId);
            stoppage.Apply(c);
            await _store.SaveChanges(stoppage);
        }
    }
}