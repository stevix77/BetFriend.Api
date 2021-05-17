namespace BetFriend.Domain.Members
{
    using System.Threading.Tasks;

    public interface IMemberRepository
    {
        Task<bool> ExistsAllAsync(MemberId[] participants);
    }
}
