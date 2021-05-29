namespace BetFriend.Infrastructure.InMemory.Repositories
{
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Guid> _memberIds;
        public InMemoryMemberRepository(List<Guid> memberIds = null)
        {
            _memberIds = memberIds ?? new List<Guid>();
        }

        public Task<bool> ExistsAllAsync(Guid[] participants)
        {
            return Task.FromResult(_memberIds.Count(x => participants.Contains(x)) == participants.Count());
        }
    }
}
