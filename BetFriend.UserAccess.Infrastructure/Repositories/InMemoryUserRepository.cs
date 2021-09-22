namespace BetFriend.UserAccess.Infrastructure.Repositories
{
    using BetFriend.UserAccess.Domain.Users;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ICollection<User> _users;

        public InMemoryUserRepository(User user = null)
        {
            _users = new List<User>();
            if (user != null)
                _users.Add(user);
        }

        public IReadOnlyCollection<User> GetUsers()
        {
            return _users.ToList();
        }

        public Task<bool> IsUserExistsAsync(string username, string email)
        {
            return Task.FromResult(_users.Any(x => x.Username == username || x.Email == email));
        }

        public Task SaveAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }
    }
}
