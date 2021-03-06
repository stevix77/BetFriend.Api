namespace BetFriend.Bet.Application.Usecases.SubscribeMember
{
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Domain.Subscriptions;
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Domain;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class SubscribeMemberCommanderHandler : ICommandHandler<SubscribeMemberCommand>
    {
        private readonly IMemberRepository _memberReposiory;
        private readonly IAuthenticationGateway _authenticationGateway;

        public SubscribeMemberCommanderHandler(IMemberRepository memberReposiory, IAuthenticationGateway authenticationGateway)
        {
            _memberReposiory = memberReposiory;
            _authenticationGateway = authenticationGateway;
        }

        public async Task<Unit> Handle(SubscribeMemberCommand command, CancellationToken cancellationToken)
        {
            if (!_authenticationGateway.IsAuthenticated())
                throw new NotAuthenticatedException();
            var currentUserId = new MemberId(_authenticationGateway.UserId);
            var members = await _memberReposiory.GetByIdsAsync(new[] { currentUserId, command.SubscriptionId }).ConfigureAwait(false);
            var member = members.SingleOrDefault(x => x.Id.Equals(currentUserId))
                            ?? throw new MemberUnknownException($"Member with id {currentUserId.Value} does not exist");
            if(!members.Any(x => x.Id.Equals(command.SubscriptionId)))
                throw new MemberUnknownException($"Member with id {command.SubscriptionId.Value} does not exist");
            member.Subscribe(new Subscription(command.SubscriptionId));
            await _memberReposiory.SaveAsync(members).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
