namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class InMemoryQueryMemberRepository : IQueryMemberRepository
    {
        private readonly List<MemberDto> _members;
        public InMemoryQueryMemberRepository(List<MemberDto> members = null)
        {
            _members = members ?? new List<MemberDto>();
        }

        public Task<MemberDto> GetByIdAsync(System.Guid memberId)
        {
            var member = _members.Find(x => x.Id == memberId);
            return Task.FromResult(member);
        }
    }
}
