namespace BetFriend.UserAccess.Infrastructure
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Application.Abstractions.Query;
    using BetFriend.UserAccess.Application.Abstractions;
    using MediatR;
    using System;
    using System.Threading.Tasks;


    public class UserAccessModule : IUserAccessModule
    {
        private readonly UserAccessCompositionRoot _userAccessCompositionRoot;
        public UserAccessModule(IServiceProvider provider)
        {
            _userAccessCompositionRoot = new UserAccessCompositionRoot(provider);
        }
        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            using (var scope = _userAccessCompositionRoot.BeginScope())
            {
                var mediator = scope.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
                return await mediator.Send(command);
            }
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = _userAccessCompositionRoot.BeginScope();
            var mediator = scope.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
            return await mediator.Send(query);
        }
    }
}
