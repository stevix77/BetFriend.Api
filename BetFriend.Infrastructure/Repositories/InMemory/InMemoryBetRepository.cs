namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Domain.Bets;
    using System;
    using System.Threading.Tasks;
    using static BetFriend.Bet.Domain.Bets.Bet;

    public sealed class InMemoryBetRepository : IBetRepository
    {
        private readonly IDomainEventsListener _domainEventsListener;
        private BetState _bet;

        public InMemoryBetRepository(IDomainEventsListener domainEventsListener = null, BetState betState = null)
        {
            _domainEventsListener = domainEventsListener;
            _bet = betState;
        }

        public Task SaveAsync(Bet bet)
        {
            _domainEventsListener.AddDomainEvents(bet.DomainEvents);
            _bet = bet.State;
            return Task.CompletedTask;
        }

        public Task<Bet> GetByIdAsync(BetId betId)
        {
            if (betId.Value.Equals(_bet?.BetId))
                return Task.FromResult(FromState(_bet));

            return Task.FromResult<Bet>(null);

        }
    }
}
