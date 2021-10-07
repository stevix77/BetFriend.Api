namespace BetFriend.Shared.Infrastructure
{
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.Shared.Domain;
    using System;
    using System.Threading.Tasks;

    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IDomainEventsAccessor _domainEventsAccessor;
        private readonly IStorageDomainEventsRepository _storageDomainEventsRepository;

        public DomainEventsDispatcher(IDomainEventsAccessor domainEventsAccessor, IStorageDomainEventsRepository storageDomainEventsRepository)
        {
            _domainEventsAccessor = domainEventsAccessor ?? throw new ArgumentNullException(nameof(domainEventsAccessor));
            _storageDomainEventsRepository = storageDomainEventsRepository ?? throw new ArgumentNullException(nameof(storageDomainEventsRepository));
        }

        public async Task DispatchEventsAsync()
        {
            foreach (var item in _domainEventsAccessor.GetDomainEvents())
            {
                await _storageDomainEventsRepository.SaveAsync(item).ConfigureAwait(false);
            }
        }
    }
}
