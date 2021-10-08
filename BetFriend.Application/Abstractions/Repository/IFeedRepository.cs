namespace BetFriend.Bet.Application.Abstractions.Repository
{
    using BetFriend.Bet.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFeedRepository
    {
        Task<IReadOnlyCollection<FeedDto>> GetByIdsAsync(IEnumerable<Guid> feedIds);
        Task SaveAsync(IReadOnlyCollection<FeedDto> feeds);
        Task SaveAsync(FeedDto feed);
    }
}
