using System.Threading.Tasks;
using Abstractions;
using Shared;
using static System.Guid;

namespace MachineJobProcessor.Domain
{
    internal sealed class MachineJobProcessorViewObserver
    {
        private readonly StartNewMachineJobHandler _startNewMachineJobHandler;

        public MachineJobProcessorViewObserver(IStore store)
        {
            _startNewMachineJobHandler = new StartNewMachineJobHandler(store);
        }

        public async Task ObserveChange(MachineJobProcessorView view)
        {
            switch (view.LastAppliedEventType)
            {
                case nameof(MachineStarted):
                case nameof(MachineJobCompleted):
                case nameof(NewMachineJobRequested):
                    await _startNewMachineJobHandler.Handle(new StartNewMachineJobCommand(view,NewGuid()));
                    break;
            }
        }
    }
}