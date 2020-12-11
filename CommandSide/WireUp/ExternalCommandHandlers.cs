using System;
using System.Threading.Tasks;
using Abstractions;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}