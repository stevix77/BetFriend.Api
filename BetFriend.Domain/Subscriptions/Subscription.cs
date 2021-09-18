namespace BetFriend.Domain.Subscriptions
{
    using BetFriend.Domain.Members;


    public class Subscription
    {
        public MemberId MemberId { get; }

        public Subscription(MemberId memberId)
        {
            MemberId = memberId;
        }
    }
}
