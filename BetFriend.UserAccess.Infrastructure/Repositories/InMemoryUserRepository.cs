namespace BetFriend.UserAccess.Infrastructure.Repositories
{
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.UserAccess.Domain.Users;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ICollection<User> _users;
        private readonly IDomainEventsListener _domainEventsListener;

        public InMemoryUserRepository(User user = null, IDomainEventsListener domainEventsListener = null)
        {
            _users = new List<User>();
            if (user != null)
                _users.Add(user);
            _domainEventsListener = domainEventsListener;
        }

        public Task<User> GetByLoginPasswordAsync(string login, string password)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.Password == password && (x.Email.ToString() == login || x.Username == login)));
        }

        public IReadOnlyCollection<User> GetUsers()
        {
            return _users.ToList();
        }

        public Task<bool> IsEmailExistsAsync(string email)
        {
            return Task.FromResult(_users.Any(x => x.Email.ToString() == email));
        }

        public Task<bool> IsUsernameExistsAsync(string username)
        {
            return Task.FromResult(_users.Any(x => x.Username == username));
        }

        public Task SaveAsync(User user)
        {
            _users.Add(user);
            _domainEventsListener?.AddDomainEvents(user.DomainEvents);
            return Task.CompletedTask;
        }
    }
}
