using System;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace Abstractions
{
    public interface IPersistedEventSource
    {
        Task SubscribeTo<T>(
            Func<T, Task> eventHandler,
            CancellationToken cancellationToken = default) where T : IEvent;
    }
}