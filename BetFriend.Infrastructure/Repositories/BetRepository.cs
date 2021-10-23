namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;
    using BetFriend.Bet.Infrastructure.Extensions;
    using BetFriend.Shared.Application.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BetRepository : IBetRepository
    {
        private readonly DbContext _dbContext;
        private readonly IDomainEventsAccessor _domainEventsAccessor;

        public BetRepository(DbContext dbContext, IDomainEventsAccessor domainEventsAccessor)
        {
            _dbContext = dbContext;
            _domainEventsAccessor = domainEventsAccessor;
        }

        public async Task SaveAsync(Bet bet)
        {
            var entity = await _dbContext.Set<BetEntity>().FindAsync(bet.State.BetId).ConfigureAwait(false);
            if (entity != null)
            {
                UpdateEntity(bet, entity);
                return;
            }
            await CreateEntity(bet);
        }

        private async Task CreateEntity(Bet bet)
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
            _domainEventsAccessor.AddDomainEvents(bet.DomainEvents);
        }

        private void UpdateEntity(Bet bet, BetEntity entity)
        {
            entity.Answers = bet.State.Answers.Select(x => new AnswerEntity
            {
                BetId = entity.BetId,
                DateAnswer = x.DateAnswer,
                MemberId = x.Member.Id.Value,
                IsAccepted = x.IsAccepted
            }).ToList();
            entity.CloseDate = bet.State.CloseDate;
            entity.IsSuccess = bet.State.IsSuccess;
            _dbContext.Set<BetEntity>().Update(entity);
            _domainEventsAccessor.AddDomainEvents(bet.DomainEvents);
        }

        public async Task<Bet> GetByIdAsync(BetId betId)
        {
            var entity = await _dbContext.Set<BetEntity>()
                                         .Include(x => x.Creator)
                                         .Include(x => x.Answers)
                                         .Include($"{nameof(BetEntity.Answers)}.{nameof(AnswerEntity.Member)}")
                                         .FirstOrDefaultAsync(x => x.BetId == betId.Value);
            return entity?.ToBet();
        }

        public async Task<IReadOnlyCollection<Bet>> GetBetsForFeedAsync()
        {
            var bets = await _dbContext.Set<BetEntity>()
                                       .Include(x => x.Creator)
                                       .AsNoTracking()
                                       .Where(x => !x.CloseDate.HasValue)
                                       .OrderBy(x => x.EndDate)
                                       .ToListAsync();
            return bets.Select(x => x.ToBet()).ToList();
        }
    }
}
