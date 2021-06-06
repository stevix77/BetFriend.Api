namespace BetFriend.Domain.Bets
{
    using System;
    using System.Threading.Tasks;


    public interface IBetRepository
    {
        Task<Bet> GetByIdAsync(Guid betId);
        Task SaveAsync(Bet bet);
    }
}
