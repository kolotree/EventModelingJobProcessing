using System.Threading.Tasks;

namespace JobProcessing.Abstractions
{
    public interface ICommandHandler<T>
    {
        Task Handle(T c);
    }
}