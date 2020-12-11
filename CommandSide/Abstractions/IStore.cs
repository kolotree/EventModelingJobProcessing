using System.Threading.Tasks;

namespace Abstractions
{
    public interface IStore
    {
        Task<T> Get<T>(StreamId streamId) where T : AggregateRoot, new();
        
        Task SaveChanges<T>(T aggregateRoot) where T : AggregateRoot;
    }
}