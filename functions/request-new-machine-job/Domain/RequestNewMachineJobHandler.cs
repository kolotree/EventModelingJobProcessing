using System;
using System.Threading.Tasks;
using Abstractions;

namespace Function.Domain
{
    public sealed class RequestNewMachineJobHandler : ICommandHandler<RequestNewMachineJob>
    {
        private readonly IStore _store;

        public RequestNewMachineJobHandler(IStore store)
        {
            _store = store;
        }
        
        public Task Handle(RequestNewMachineJob c)
        {
            return _store.SaveChanges(NewMachineJobRequest.From(c, DateTime.UtcNow));
        }
    }
}