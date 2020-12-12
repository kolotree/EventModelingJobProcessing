using System.Threading.Tasks;
using Abstractions;

namespace CreateMachine
{
    public sealed class CreateMachineHandler : ICommandHandler<CreateMachineCommand>
    {
        private readonly IStore _store;

        public CreateMachineHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(CreateMachineCommand c)
        {
            return _store.SaveChanges(Machine.NewOf(c));
        }
    }
}