namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Guid> _memberIds;
        private List<Member> _members;

        public InMemoryMemberRepository(List<Guid> memberIds = null)
        {
            _memberIds = memberIds ?? new List<Guid>();
        }

        public InMemoryMemberRepository(List<Member> members)
        {
            _members = members;
        }

        public Task<Member> GetByIdAsync(Guid memberId)
        {
            return Task.FromResult(_members.Find(x => x.CreatorId == memberId));
        }
    }
}
