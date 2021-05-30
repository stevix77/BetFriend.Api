using System.Collections.Generic;

namespace BetFriend.Domain
{
    public abstract class Entity
    {
        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get => _domainEvents?.AsReadOnly(); }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }
    }
}
