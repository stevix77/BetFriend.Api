namespace BetFriend.Bet.Infrastructure
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Application.Abstractions.Query;
    using MediatR;
    using System;
    using System.Threading.Tasks;


    public class BetModule : IBetModule
    {
        private readonly BetCompositionRoot _betCompositionRoot;
        public BetModule(IServiceProvider provider)
        {
            _betCompositionRoot = new BetCompositionRoot(provider);
        }

        public async Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command)
        {
            using var scope = _betCompositionRoot.BeginScope();
            var mediator = scope.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
            await mediator.Send(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = _betCompositionRoot.BeginScope();
            var mediator = scope.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
            return await mediator.Send(query);
        }
    }
}
