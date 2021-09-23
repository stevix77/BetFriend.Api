using System.Threading.Tasks;

namespace BetFriend.Shared.Infrastructure
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        public Task Commit()
        {
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }
    }
}
