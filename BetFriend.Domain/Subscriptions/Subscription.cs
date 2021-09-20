namespace BetFriend.Bet.Domain.Subscriptions
{
    using BetFriend.Bet.Domain.Members;


    public class Subscription
    {
        public MemberId MemberId { get; }

        public Subscription(MemberId memberId)
        {
            MemberId = memberId;
        }
    }
}
