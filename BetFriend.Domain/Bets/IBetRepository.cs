namespace BetFriend.Domain.Bets
{
    using System.Threading.Tasks;


    public interface IBetRepository
    {
        Task<Bet> GetByIdAsync(BetId betId);
        Task AddAsync(Bet bet);
    }
}
