namespace BetFriend.UserAccess.Domain.Users
{
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task SaveAsync(User user);
    }
}
