using BetFriend.Bet.Application.Abstractions.Command;
using BetFriend.Bet.Application.Abstractions.Query;
using System.Threading.Tasks;

namespace BetFriend.Bet.Application.Abstractions
{
    public interface IProcessor
    {
        Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command);
        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
