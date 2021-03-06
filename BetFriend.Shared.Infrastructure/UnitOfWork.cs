namespace BetFriend.Shared.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(DbContext dbContext, IDomainEventsDispatcher domainEventsDispatcher)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
            _domainEventsDispatcher = domainEventsDispatcher ?? throw new System.ArgumentNullException(nameof(domainEventsDispatcher));
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            await _domainEventsDispatcher.DispatchEventsAsync();
        }

        public async Task Rollback()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
