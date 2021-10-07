namespace BetFriend.Shared.Application.Abstractions
{
    using BetFriend.Shared.Domain;
    using System.Collections.Generic;

    public interface IDomainEventsAccessor
    {
        void AddDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents);
        IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    }
}
