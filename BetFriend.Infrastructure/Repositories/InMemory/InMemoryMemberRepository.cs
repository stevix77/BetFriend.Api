namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Member> _members;

        public InMemoryMemberRepository(List<Member> members = null)
        {
            _members = members ?? (_members = new List<Member>());
        }

        public Task<Member> GetByIdAsync(MemberId memberId)
        {
            return Task.FromResult(_members.Find(x => x.Id.Value == memberId.Value));
        }

        public Task<List<Member>> GetByIdsAsync(IEnumerable<Guid> memberIds)
        {
            return Task.FromResult(_members.Where(x => memberIds.Contains(x.Id.Value))
                                           .ToList());
        }

        public Task SaveAsync(IReadOnlyCollection<Member> members)
        {
            return Task.CompletedTask;
        }
    }
}
