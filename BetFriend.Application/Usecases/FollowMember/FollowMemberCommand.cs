namespace BetFriend.Application.Usecases.FollowMember
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Domain.Members;
    using System;


    public sealed class FollowMemberCommand : ICommand
    {
        private Guid _memberId;
        private Guid _memberIdToFollow;

        public FollowMemberCommand(Guid memberId, Guid memberIdToFollow)
        {
            _memberId = memberId;
            _memberIdToFollow = memberIdToFollow;
        }

        public MemberId MemberId { get => new(_memberId); }
        public MemberId MemberIdToFollow { get => new(_memberIdToFollow); }
    }
}
