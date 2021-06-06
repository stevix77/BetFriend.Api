namespace BetFriend.Infrastructure
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

        public async Task BeginTransaction()
        {
            await _dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
        }

        public async Task Commit()
        {
            await _dbContext.Database.CommitTransactionAsync().ConfigureAwait(false);
        }

        public async Task Rollback()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
