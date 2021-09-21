namespace BetFriend.UserAccess.Infrastructure.Repositories
{
    using BetFriend.UserAccess.Domain.Users;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ICollection<User> _users;

        public InMemoryUserRepository()
        {
            _users = new List<User>();
        }

        public IReadOnlyCollection<User> GetUsers()
        {
            return _users.ToList();
        }

        public Task SaveAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }
    }
}
