namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Domain.Bets;
    using System;
    using System.Threading.Tasks;
    using static BetFriend.Domain.Bets.Bet;

    public sealed class InMemoryBetRepository : IBetRepository
    {
        private BetState _bet;
        public Task AddAsync(Bet bet)
        {
            _bet = bet?.State ?? throw new ArgumentNullException(nameof(bet));
            return Task.CompletedTask;
        }

        public Task<Bet> GetByIdAsync(Guid betId)
        {
            if (betId.Equals(_bet.BetId))
                return Task.FromResult(FromState(_bet));

            return Task.FromResult<Bet>(null);

        }
    }
}
