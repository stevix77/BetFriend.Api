using BetFriend.Application.Abstractions.Command;
using System.Threading.Tasks;

namespace BetFriend.Application.Abstractions
{
    public interface IProcessor
    {
        Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command);

    }
}
