namespace BetFriend.Application.Abstractions
{
    using BetFriend.Domain;
    using System.Collections.Generic;

    public interface IDomainEventsListener
    {
        void AddDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents);
        IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    }
}
