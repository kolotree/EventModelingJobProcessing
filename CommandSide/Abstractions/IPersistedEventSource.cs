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
        
        Task SubscribeToView<T>(
            Func<T, Task> viewHandler,
            CancellationToken cancellationToken = default) where T : IView;
    }
}