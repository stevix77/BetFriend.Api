using BetFriend.Bet.Application.Abstractions;
using BetFriend.Bet.Domain;
using System.Collections.Generic;

namespace BetFriend.Bet.Application
{
    public sealed class DomainEventsListener : IDomainEventsListener
    {
        private readonly List<IDomainEvent> _domainEvents;
        public DomainEventsListener()
        {
            _domainEvents = new List<IDomainEvent>();
        }
        public void AddDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            _domainEvents.AddRange(domainEvents);
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
    }
}
