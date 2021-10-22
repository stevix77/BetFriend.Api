namespace BetFriend.Bet.Domain.Bets
{
    using System;

    public abstract class Status
    {
        public virtual bool IsClosed() => false;
        public virtual DateTime? GetCloseDate() => null;
        internal virtual void ChangeStatus(Bet bet) { }
        public virtual bool? IsSuccess() => null;
    }
}
