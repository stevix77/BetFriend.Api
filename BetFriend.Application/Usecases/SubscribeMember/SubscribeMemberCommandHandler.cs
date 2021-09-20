namespace BetFriend.Bet.Application.Usecases.SubscribeMember
{
    using BetFriend.Bet.Application.Abstractions.Command;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Domain.Subscriptions;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class SubscribeMemberCommanderHandler : ICommandHandler<SubscribeMemberCommand>
    {
        private IMemberRepository _memberReposiory;

        public SubscribeMemberCommanderHandler(IMemberRepository memberReposiory)
        {
            _memberReposiory = memberReposiory;
        }

        public async Task<Unit> Handle(SubscribeMemberCommand command, CancellationToken cancellationToken)
        {
            var members = await _memberReposiory.GetByIdsAsync(new[] { command.MemberId, command.SubscriptionId }).ConfigureAwait(false);
            var member = members.SingleOrDefault(x => x.Id.Equals(command.MemberId))
                            ?? throw new MemberUnknownException($"Member with id {command.MemberId.Value} does not exist");
            var memberToFollow = members.SingleOrDefault(x => x.Id.Equals(command.SubscriptionId))
                            ?? throw new MemberUnknownException($"Member with id {command.SubscriptionId.Value} does not exist");
            member.Subscribe(new Subscription(command.SubscriptionId));
            await _memberReposiory.SaveAsync(members).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
