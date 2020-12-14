using System.Threading.Tasks;

namespace Abstractions
{
    public interface ICommandHandler<T>
    {
        Task Handle(T c);
    }
}