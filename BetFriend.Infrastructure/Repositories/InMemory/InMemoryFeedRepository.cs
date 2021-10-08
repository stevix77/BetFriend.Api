namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryFeedRepository : IFeedRepository
    {
        private readonly List<FeedDto> _feeds;

        public InMemoryFeedRepository(IEnumerable<FeedDto> feeds = null)
        {
            _feeds = feeds != null ? feeds.ToList() : new List<FeedDto>();
        }

        public FeedDto GetById(Guid feedId)
        {
            return _feeds.FirstOrDefault(x => x.Id == feedId.ToString());
        }

        public async Task<IReadOnlyCollection<FeedDto>> GetByIdsAsync(IEnumerable<Guid> feedIds)
        {
            return await Task.FromResult(_feeds.Where(x => feedIds.Contains(Guid.Parse(x.Id))).ToList());
        }

        public IEnumerable<FeedDto> GetFeeds() => _feeds;

        public Task SaveAsync(FeedDto feed)
        {
            _feeds.Add(feed);
            return Task.CompletedTask;
        }

        public Task SaveAsync(IReadOnlyCollection<FeedDto> feeds)
        {
            _feeds.AddRange(feeds);
            return Task.CompletedTask;
        }
    }
}
