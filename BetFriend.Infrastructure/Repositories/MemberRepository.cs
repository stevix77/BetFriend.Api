namespace BetFriend.Infrastructure.Repositories
{
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;


    public class MemberRepository : IMemberRepository
    {
        private readonly DbContext _dbContext;

        public MemberRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Member> GetByIdAsync(Guid memberId)
        {
            var entity = await _dbContext.FindAsync<MemberEntity>(memberId).ConfigureAwait(false);
            return entity == null ? null :
                                    new Member(new MemberId(entity.MemberId), entity.MemberName, entity.Wallet);
        }
    }
}
