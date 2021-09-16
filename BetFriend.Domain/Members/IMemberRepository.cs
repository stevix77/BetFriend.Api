namespace BetFriend.Domain.Members
{
    using System;
    using System.Threading.Tasks;

    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(MemberId memberId);
    }
}
