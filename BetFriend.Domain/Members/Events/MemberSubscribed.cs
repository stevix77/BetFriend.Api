namespace BetFriend.Bet.Domain.Members.Events
{
    using System;


    public class MemberSubscribed : IDomainEvent
    {
        public MemberSubscribed(MemberId memberId, MemberId subscriptionId)
        {
            MemberId = memberId.Value;
            SubscriptionId = subscriptionId.Value;
        }

        public Guid MemberId { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
