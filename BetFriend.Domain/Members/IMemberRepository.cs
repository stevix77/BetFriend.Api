namespace BetFriend.Domain.Members
{
    using System;
    using System.Threading.Tasks;

    public interface IMemberRepository
    {
        Task<bool> ExistsAllAsync(Guid[] participants);
    }
}
