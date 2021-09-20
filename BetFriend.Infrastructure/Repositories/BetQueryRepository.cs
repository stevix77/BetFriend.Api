namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class BetQueryRepository : IBetQueryRepository
    {
        private readonly IMongoCollection<BetDto> _collection;
        public BetQueryRepository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<BetDto>(nameof(BetDto));
        }

        public async Task<IReadOnlyCollection<BetDto>> GetBetsForMemberAsync(Guid memberId)
        {
            var bets = (await _collection.FindAsync(x => x.Creator.Id == memberId
                                            || x.Participants.Any(y => y.Id == memberId))
                                        .ConfigureAwait(false))
                                        .ToListAsync();

            return await bets;

            
        }

        public async Task<BetDto> GetByIdAsync(Guid betId)
        {
            var betDto = _collection.Find(x => x.Id == betId).FirstOrDefaultAsync();
            return await betDto.ConfigureAwait(false);

        }

        public async Task SaveAsync(BetDto betDto)
        {
            var result = _collection.FindOneAndReplace(x => x.Id == betDto.Id, betDto);
            if(result == null)
                await _collection.InsertOneAsync(betDto).ConfigureAwait(false);
        }
    }
}
