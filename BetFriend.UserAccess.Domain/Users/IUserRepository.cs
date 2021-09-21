using System.Threading.Tasks;

namespace BetFriend.UserAccess.Domain.Users
{
    public interface IUserRepository
    {
        Task SaveAsync(User user);
    }
}
