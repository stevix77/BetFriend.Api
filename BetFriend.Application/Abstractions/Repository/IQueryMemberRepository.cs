using BetFriend.Bet.Application.Models;
using System.Threading.Tasks;

namespace BetFriend.Bet.Application.Abstractions.Repository
{
    public interface IQueryMemberRepository
    {
        Task<MemberDto> GetByIdAsync(System.Guid memberId);
    }
}
