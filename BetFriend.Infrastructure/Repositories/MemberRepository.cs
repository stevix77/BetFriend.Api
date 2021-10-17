namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;
    using BetFriend.Shared.Application.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class MemberRepository : IMemberRepository
    {
        private readonly DbContext _dbContext;
        private readonly IDomainEventsAccessor _domainEventsAccessor;

        public MemberRepository(DbContext dbContext, IDomainEventsAccessor domainEventsAccessor)
        {
            _dbContext = dbContext;
            _domainEventsAccessor = domainEventsAccessor;
        }

        public async Task<Member> GetByIdAsync(MemberId memberId)
        {
            var entity = await _dbContext.FindAsync<MemberEntity>(memberId.Value).ConfigureAwait(false);
            return entity == null ? null :
                                    new Member(new MemberId(entity.MemberId), entity.MemberName, entity.Wallet);
        }

        public Task<List<Member>> GetByIdsAsync(IEnumerable<MemberId> participantsId)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(IReadOnlyCollection<Member> members)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(Member member)
        {
            await _dbContext.AddAsync(new MemberEntity
            {
                MemberId = member.Id.Value,
                MemberName = member.Name,
                Wallet = member.Wallet
            });
            _domainEventsAccessor.AddDomainEvents(member.DomainEvents);
        }
    }
}
