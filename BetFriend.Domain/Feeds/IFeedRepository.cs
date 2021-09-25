namespace BetFriend.Bet.Domain.Feeds
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFeedRepository
    {
        Task<Feed> GetByIdAsync(Guid feedId);
        Task SaveAsync(IReadOnlyCollection<Feed> feeds);
        Task<IReadOnlyCollection<Feed>> GetByIdsAsync(IEnumerable<Guid> enumerable);
        Task SaveAsync(Feed feed);
    }
}
