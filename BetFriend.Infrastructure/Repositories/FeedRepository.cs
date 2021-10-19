namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class FeedRepository : IFeedRepository
    {
        private readonly IMongoCollection<FeedDto> _collection;

        public FeedRepository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<FeedDto>(nameof(FeedDto));
        }

        public async Task<IReadOnlyCollection<FeedDto>> GetByIdsAsync(IEnumerable<Guid> feedIds)
        {
            return await _collection.Find(Builders<FeedDto>.Filter.In(x => x.Id, feedIds.Select(x => x.ToString())))
                                    .ToListAsync()
                                    .ConfigureAwait(false);
        }

        public Task SaveAsync(IReadOnlyCollection<FeedDto> feeds)
        {
            foreach(var feed in feeds)
                _collection.FindOneAndReplaceAsync(x => x.Id == feed.Id, feed);
            return Task.CompletedTask;
        }

        public async Task SaveAsync(FeedDto feed)
        {
            await _collection.InsertOneAsync(feed).ConfigureAwait(false);
        }
    }
}
