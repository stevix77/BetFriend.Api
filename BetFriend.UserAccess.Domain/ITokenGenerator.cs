using BetFriend.UserAccess.Domain.Users;
using System.Threading.Tasks;

namespace BetFriend.UserAccess.Domain
{
    public interface ITokenGenerator
    {
        Task<string> GenerateAsync(User user);
    }
}
