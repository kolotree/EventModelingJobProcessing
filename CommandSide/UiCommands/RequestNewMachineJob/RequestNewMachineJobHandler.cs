using System;
using System.Threading.Tasks;
using Abstractions;

namespace RequestNewMachineJob
{
    public sealed class RequestNewMachineJobHandler : ICommandHandler<RequestNewMachineJobCommand>
    {
        private readonly IStore _store;

        public RequestNewMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(RequestNewMachineJobCommand c)
        {
            return _store.SaveChanges(NewMachineJobRequest.From(c, DateTime.UtcNow));
        }
    }
}