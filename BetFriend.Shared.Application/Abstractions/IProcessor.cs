namespace BetFriend.Shared.Application.Abstractions
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Application.Abstractions.Query;
    using System.Threading.Tasks;


    public interface IProcessor
    {
        Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command);
        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
