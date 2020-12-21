using System;
using System.Threading.Tasks;
using Abstractions;
using CompleteMachineStoppage;
using DetectMachineCycles;
using DetectMachineStart;
using DetectMachineStop;
using RequestNewMachineJob;

namespace CommandsWireUp
{
    public sealed class ExternalCommandHandlers : ICommandHandler<ICommand>
    {
        private readonly IStore _store;

        private ExternalCommandHandlers(IStore store)
        {
            _store = store;
        }
        
        public static ExternalCommandHandlers NewWith(IStore store) => new(store);

        public Task Handle(ICommand c) =>
            c switch
            {
                DetectMachineStopCommand detectMachineStopCommand => new DetectMachineStopHandler(_store).Handle(detectMachineStopCommand),
                DetectMachineStartCommand detectMachineStartCommand => new DetectMachineStartHandler(_store).Handle(detectMachineStartCommand),
                CompleteMachineJobCommand completeMachineJobCommand => new CompleteMachineJobHandler(_store).Handle(completeMachineJobCommand),
                RequestNewMachineJobCommand requestNewMachineJobCommand => new RequestNewMachineJobHandler(_store).Handle(requestNewMachineJobCommand),
                DetectMachineCyclesCommand detectMachineCycleCommand => new DetectMachineCyclesHandler(_store).Handle(detectMachineCycleCommand),
                _ => throw new ArgumentOutOfRangeException(nameof(c))
            };
    }
}