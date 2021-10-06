using BetFriend.Bet.Application.Abstractions;
using BetFriend.Shared.Application.Abstractions.Command;
using BetFriend.Shared.Application.Abstractions.Query;
using MediatR;
using System.Threading.Tasks;

namespace BetFriend.Bet.Infrastructure
{
    public class BetProcessor : IBetProcessor
    {
        public async Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command)
        {
            using var scope = BetCompositionRoot.BeginScope();
            var mediator = scope.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
            await mediator.Send(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = BetCompositionRoot.BeginScope();
            var mediator = scope.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
            return await mediator.Send(query);
        }
    }
}
