using BetFriend.Bet.Application.Abstractions.Repository;
using BetFriend.Bet.Application.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace BetFriend.Bet.Infrastructure.Repositories
{
    public class MemberQueryRepository : IQueryMemberRepository
    {
        private readonly IMongoCollection<MemberDto> _collection;
        public MemberQueryRepository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<MemberDto>(nameof(MemberDto));
        }

        public async Task<MemberDto> GetByIdAsync(Guid memberId)
        {
            return await _collection.Find(x => x.Id.Equals(memberId)).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
