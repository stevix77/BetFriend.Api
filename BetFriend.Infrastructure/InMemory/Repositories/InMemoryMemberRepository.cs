namespace BetFriend.Infrastructure.InMemory.Repositories
{
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<MemberId> _memberIds;
        public InMemoryMemberRepository(List<MemberId> memberIds = null)
        {
            _memberIds = memberIds ?? new List<MemberId>();
        }

        public Task<bool> ExistsAllAsync(MemberId[] participants)
        {
            return Task.FromResult(_memberIds.Count(x => participants.Contains(x)) == participants.Count());
        }
    }
}
