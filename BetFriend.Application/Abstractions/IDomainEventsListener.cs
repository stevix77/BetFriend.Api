﻿namespace BetFriend.Bet.Application.Abstractions
{
    using BetFriend.Bet.Domain;
    using System.Collections.Generic;

    public interface IDomainEventsListener
    {
        void AddDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents);
        IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    }
}
