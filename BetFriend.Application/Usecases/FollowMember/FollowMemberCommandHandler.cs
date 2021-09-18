namespace BetFriend.Application.Usecases.FollowMember
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Domain.Members;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class FollowMemberCommanderHandler : ICommandHandler<FollowMemberCommand>
    {
        private IMemberRepository _memberReposiory;

        public FollowMemberCommanderHandler(IMemberRepository memberReposiory)
        {
            _memberReposiory = memberReposiory;
        }

        public async Task<Unit> Handle(FollowMemberCommand command, CancellationToken cancellationToken)
        {
            var member = await _memberReposiory.GetByIdAsync(command.MemberId).ConfigureAwait(false);
            member.AddFollower(new Domain.Followers.Follower(command.MemberIdToFollow));

            return Unit.Value;
        }
    }
}
