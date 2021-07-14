namespace BetFriend.Infrastructure.Repositories
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.ViewModels;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class BetQueryRepository : IBetQueryRepository
    {
        private readonly IMongoCollection<BetViewModel> _collection;
        public BetQueryRepository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<BetViewModel>(nameof(BetViewModel));
        }

        public async Task<IReadOnlyCollection<BetViewModel>> GetBetsForMemberAsync(Guid memberId)
        {
            var bets = (await _collection.FindAsync(x => x.CreatorId == memberId
                                            || x.Participants.Any(y => y.Id == memberId))
                                        .ConfigureAwait(false))
                                        .ToListAsync();

            return await bets;

            
        }

        public async Task SaveAsync(BetViewModel betViewModel)
        {
            await _collection.InsertOneAsync(betViewModel);
        }
    }
}
