using BetFriend.Application.Abstractions.Command;
using BetFriend.Application.Abstractions.Query;
using System.Threading.Tasks;

namespace BetFriend.Application.Abstractions
{
    public interface IProcessor
    {
        Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command);
        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
