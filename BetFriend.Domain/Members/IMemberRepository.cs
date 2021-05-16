using System.Threading.Tasks;

namespace BetFriend.Domain.Members
{
    public interface IMemberRepository
    {
        Task<bool> ExistsAllAsync(MemberId[] participants);
    }
}
