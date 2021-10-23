namespace BetFriend.Bet.Domain.Bets
{
    public class BetOpenStatus : Status
    {
        internal override void ChangeStatus(Bet bet)
        {
            bet.ChangeStatus(new BetOverStatus());
        }

        internal override bool IsClosed() => false;
    }
}
