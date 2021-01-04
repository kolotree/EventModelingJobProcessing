using System.Threading.Tasks;

namespace Abstractions
{
    public interface IStore
    {
        Task<T> Get<T>(StreamId streamId) where T : Stream, new();
        
        Task SaveChanges<T>(T stream) where T : Stream;

        Task AppendTo<T>(T stream) where T : IStream;
    }
}