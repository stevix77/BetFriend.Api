namespace BetFriend.Bet.Domain.Members.Events
{
    using BetFriend.Shared.Domain;
    using System;

    public class MemberCreated : IDomainEvent
    {
        public MemberCreated(Guid memberId)
        {
            MemberId = memberId;
        }

        public Guid MemberId { get; }
    }
}
