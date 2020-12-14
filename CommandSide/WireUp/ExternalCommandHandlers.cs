using System;
using System.Threading.Tasks;
using Abstractions;
using CompleteMachineStoppage;
using DetectMachineStart;
using DetectMachineStop;

namespace CommandsWireUp
{
    public sealed class ExternalCommandHandlers : ICommandHandler<ICommand>
    {
        private readonly IStore _store;

        private ExternalCommandHandlers(IStore store)
        {
            _store = store;
        }
        
        public static ExternalCommandHandlers NewWith(IStore store) => new ExternalCommandHandlers(store);

        public Task Handle(ICommand c)
        {
            switch (c)
            {
                case DetectMachineStopCommand detectMachineStopCommand: 
                    return new DetectMachineStopHandler(_store).Handle(detectMachineStopCommand);
                case DetectMachineStartCommand detectMachineStartCommand:
                    return new DetectMachineStartHandler(_store).Handle(detectMachineStartCommand);
                case CompleteMachineJobCommand completeMachineJobCommand:
                    return new CompleteMachineJobHandler(_store).Handle(completeMachineJobCommand);
                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}