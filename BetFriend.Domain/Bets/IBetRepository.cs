namespace BetFriend.Domain.Bets
{
    using System;
    using System.Threading.Tasks;


    public interface IBetRepository
    {
        Task<Bet> GetByIdAsync(BetId betId);
        Task SaveAsync(Bet bet);
    }
}
