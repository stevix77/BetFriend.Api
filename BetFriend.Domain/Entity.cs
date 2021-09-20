using System.Collections.Generic;

namespace BetFriend.Bet.Domain
{
    public abstract class Entity
    {
        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get => _domainEvents.AsReadOnly(); }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
