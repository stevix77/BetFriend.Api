namespace BetFriend.Bet.Application.Usecases.CreateMember
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;

    public sealed class CreateMemberCommand : ICommand
    {
        public CreateMemberCommand(Guid memberId, string memberName)
        {
            MemberId = memberId;
            MemberName = memberName;
        }

        public Guid MemberId { get; }
        public string MemberName { get; }
    }
}
