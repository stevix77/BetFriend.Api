﻿namespace BetFriend.Domain.Members
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(MemberId memberId);
        Task<List<Member>> GetByIdsAsync(IEnumerable<Guid> participantsId);
        Task SaveAsync(IReadOnlyCollection<Member> members);
    }
}
