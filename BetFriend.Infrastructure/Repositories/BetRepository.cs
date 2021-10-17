namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;
    using BetFriend.Bet.Infrastructure.Extensions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BetRepository : IBetRepository
    {
        private readonly DbContext _dbContext;

        public BetRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(Bet bet)
        {
            var entity = new BetEntity
            {
                BetId = bet.State.BetId,
                CreationDate = bet.State.CreationDate,
                Description = bet.State.Description,
                EndDate = bet.State.EndDate,
                Coins = bet.State.Coins,
                CreatorId = bet.State.Creator.Id.Value
            };
            await _dbContext.Set<BetEntity>().AddAsync(entity);
        }

        public async Task<Bet> GetByIdAsync(BetId betId)
        {
            var entity = await _dbContext.Set<BetEntity>()
                                         .FirstOrDefaultAsync(x => x.BetId == betId.Value);
            return entity?.ToBet();
        }

        public async Task<IReadOnlyCollection<Bet>> GetBetsForFeedAsync()
        {
            var bets = await _dbContext.Set<BetEntity>()
                                       .AsNoTracking()
                                       .Where(x => !x.IsClosed)
                                       .OrderBy(x => x.EndDate)
                                       .ToListAsync();
            return bets.Select(x => x.ToBet()).ToList();
        }
    }
}
