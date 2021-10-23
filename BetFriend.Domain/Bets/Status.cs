namespace BetFriend.Bet.Domain.Bets
{
    public abstract class Status
    {
        internal abstract void ChangeStatus(Bet bet);
        internal abstract bool IsClosed();
    }
}
