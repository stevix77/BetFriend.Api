namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private List<Member> _members;

        public InMemoryMemberRepository(List<Member> members = null)
        {
            _members = members;
        }

        public Task<Member> GetByIdAsync(Guid memberId)
        {
            return Task.FromResult(_members.Find(x => x.MemberId == memberId));
        }
    }
}
