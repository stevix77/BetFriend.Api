namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class InMemoryFeedQueryRepository : IFeedQueryRepository
    {
        private readonly List<FeedDto> _feeds;

        public InMemoryFeedQueryRepository(List<FeedDto> feeds = null)
        {
            _feeds = feeds ?? new List<FeedDto>();
        }
        public IReadOnlyCollection<FeedDto> GetFeeds() => _feeds;

        public Task SaveAsync(FeedDto feedDto)
        {
            _feeds.Add(feedDto);
            return Task.CompletedTask;
        }
    }
}
