namespace BetFriend.Shared.Infrastructure
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Domain;
    using System;
    using System.Threading.Tasks;

    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IDomainEventsListener _domainEventsListener;
        private readonly IStorageDomainEventsRepository _storageDomainEventsRepository;

        public DomainEventsDispatcher(IDomainEventsListener domainEventsListener, IStorageDomainEventsRepository storageDomainEventsRepository)
        {
            _domainEventsListener = domainEventsListener ?? throw new ArgumentNullException(nameof(domainEventsListener));
            _storageDomainEventsRepository = storageDomainEventsRepository ?? throw new ArgumentNullException(nameof(storageDomainEventsRepository));
        }

        public async Task DispatchEventsAsync()
        {
            foreach(var item in _domainEventsListener.GetDomainEvents())
            {
                await _storageDomainEventsRepository.SaveAsync(item).ConfigureAwait(false);
            }
        }
    }
}
