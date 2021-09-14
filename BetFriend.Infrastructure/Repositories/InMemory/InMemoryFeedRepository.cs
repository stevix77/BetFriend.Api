﻿namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Domain.Feeds;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryFeedRepository : IFeedRepository
    {
        private readonly List<Feed> _feeds;

        public InMemoryFeedRepository(IEnumerable<Feed> feeds = null)
        {
            _feeds = feeds != null ? feeds.ToList() : new List<Feed>();
        }

        public Task<Feed> GetByIdAsync(Guid feedId)
        {
            return Task.FromResult(_feeds.FirstOrDefault(x => x.Id == feedId));
        }

        public async Task<IReadOnlyCollection<Feed>> GetByIdsAsync(IEnumerable<Guid> feedIds)
        {
            return await Task.FromResult(_feeds.Where(x => feedIds.Contains(x.Id)).ToList());
        }

        public Task SaveAsync(IReadOnlyCollection<Feed> feeds)
        {
            return Task.CompletedTask;
        }
    }
}