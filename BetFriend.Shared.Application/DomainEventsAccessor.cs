namespace BetFriend.Shared.Application
{
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.Shared.Domain;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public sealed class DomainEventsAccessor : IDomainEventsAccessor
    {
        private readonly List<IDomainEvent> _domainEvents;
        
        public DomainEventsAccessor()
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
