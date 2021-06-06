namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Domain;
    using BetFriend.Domain.Bets;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static BetFriend.Domain.Bets.Bet;

    public sealed class InMemoryBetRepository : IBetRepository
    {
        private BetState _bet;
        private List<IDomainEvent> _domainEvents;

        public InMemoryBetRepository()
        {
            _domainEvents = new List<IDomainEvent>();
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents { get => _domainEvents.AsReadOnly(); }

        public Task SaveAsync(Bet bet)
        {
            _domainEvents.AddRange(bet.DomainEvents);
            _bet = bet.State;
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
