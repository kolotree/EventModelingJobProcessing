using System.Threading.Tasks;

namespace Abstractions
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T c);
    }
}