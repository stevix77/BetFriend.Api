namespace BetFriend.UserAccess.Domain.Users
{
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task SaveAsync(User user);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task<User> GetByLoginPasswordAsync(string login, string password);
    }
}
