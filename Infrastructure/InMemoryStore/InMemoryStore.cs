using System.Threading.Tasks;
using JobProcessing.Abstractions;

namespace JobProcessing.InMemoryStore
{
    public sealed class InMemoryStore : IStore
    {
        public Task<T> Get<T>(StreamId streamId) where T : Stream, new()
        {
            throw new System.NotImplementedException();
        }

        public Task SaveChanges<T>(T stream) where T : Stream
        {
            throw new System.NotImplementedException();
        }

        public Task AppendTo<T>(T stream) where T : IStream
        {
            throw new System.NotImplementedException();
        }
    }
}