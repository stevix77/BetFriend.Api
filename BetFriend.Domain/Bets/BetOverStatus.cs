namespace BetFriend.Bet.Domain.Bets
{

    public class BetOverStatus : Status
    {
        public BetOverStatus()
        {
        }

        internal override void ChangeStatus(Bet bet)
        {
        }

        internal override bool IsClosed() => true;
    }
}
