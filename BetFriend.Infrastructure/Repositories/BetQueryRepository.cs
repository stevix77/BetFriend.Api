namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Shared.Domain;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class BetQueryRepository : IBetQueryRepository
    {
        private readonly IMongoCollection<BetDto> _collection;
        private readonly IDateTimeProvider _dateTimeProvider;

        public BetQueryRepository(IMongoDatabase mongoDatabase, IDateTimeProvider dateTimeProvider)
        {
            _collection = mongoDatabase.GetCollection<BetDto>(nameof(BetDto));
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IReadOnlyCollection<BetDto>> GetBetsForMemberAsync()
        {
            var bets = (await _collection.FindAsync(x => x.EndDate.CompareTo(_dateTimeProvider.Now) > 0)
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
            if (result == null)
                await _collection.InsertOneAsync(betDto).ConfigureAwait(false);
        }
    }
}
