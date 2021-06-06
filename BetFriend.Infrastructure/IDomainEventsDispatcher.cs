using BetFriend.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetFriend.Infrastructure
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync(IReadOnlyCollection<IDomainEvent> events);
    }
}
