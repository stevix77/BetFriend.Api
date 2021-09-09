namespace BetFriend.Infrastructure.Repositories
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
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
            var bets = (await _collection.FindAsync(x => x.CreatorId == memberId
                                            || x.Participants.Any(y => y.Id == memberId))
                                        .ConfigureAwait(false))
                                        .ToListAsync();

            return await bets;

            
        }

        public async Task SaveAsync(BetState state, Member member)
        {
            var betDto = new BetDto
            {
                Coins = state.Coins,
                CreatorId = member.MemberId.Value,
                CreatorUsername = member.MemberName,
                Description = state.Description,
                EndDate = state.EndDate,
                Id = state.BetId
            };
            await _collection.InsertOneAsync(betDto).ConfigureAwait(false);
        }
    }
}
