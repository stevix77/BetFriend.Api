namespace BetFriend.Bet.Infrastructure.Repositories
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;
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
            var entity = await _dbContext.Set<BetEntity>().FirstOrDefaultAsync(x => x.BetId == betId.Value);
            return entity == null ? null :
                                    Bet.FromState(
                                        new BetState(betId.Value,
                                                    new Member(new MemberId(entity.Creator.MemberId),
                                                                entity.Creator.MemberName,
                                                                entity.Creator.Wallet),
                                                    entity.EndDate,
                                                    entity.Description,
                                                    entity.Coins,
                                                    entity.CreationDate,
                                                    entity.Answers?.Select(x =>
                                                        new AnswerState(new Member(new MemberId(x.Member.MemberId), x.Member.MemberName, x.Member.Wallet),
                                                                        x.IsAccepted,
                                                                        x.DateAnswer))
                                                                   .ToList()));
        }

        public Task<IReadOnlyCollection<Bet>> GetBetsForFeedAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
