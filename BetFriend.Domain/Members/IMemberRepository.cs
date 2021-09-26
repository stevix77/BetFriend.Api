namespace BetFriend.Bet.Domain.Members
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(MemberId memberId);
        Task<List<Member>> GetByIdsAsync(IEnumerable<MemberId> participantsId);
        Task SaveAsync(IReadOnlyCollection<Member> members);
        Task SaveAsync(Member member);
    }
}
