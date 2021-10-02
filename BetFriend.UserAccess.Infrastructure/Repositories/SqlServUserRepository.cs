namespace BetFriend.UserAccess.Infrastructure.Repositories
{
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class SqlServUserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;

        public SqlServUserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetByLoginPasswordAsync(string login, string password)
        {
            var user = await _dbContext.Set<UserEntity>()
                                   .FirstOrDefaultAsync(x => (x.Username == login || x.Email == login) &&
                                                            x.Password == password).ConfigureAwait(false);
            return user == null ? null : User.Restore(user.UserId,
                                                      user.Username,
                                                      user.Email,
                                                      user.Password);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _dbContext.Set<UserEntity>().AnyAsync(x => x.Email == email).ConfigureAwait(false);
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _dbContext.Set<UserEntity>().AnyAsync(x => x.Username == username).ConfigureAwait(false);
        }

        public async Task SaveAsync(User user)
        {
            var userEntity = new UserEntity
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email.ToString(),
                Password = user.Password,
                RegisterDate = user.RegisterDate
            };
            await _dbContext.AddAsync(userEntity).ConfigureAwait(false);
        }
    }
}
