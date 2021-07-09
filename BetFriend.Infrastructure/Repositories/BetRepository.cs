﻿namespace BetFriend.Infrastructure.Repositories
{
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
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
                MemberId = bet.State.CreatorId
            };
            await _dbContext.Set<BetEntity>().AddAsync(entity);
        }

        public async Task<Bet> GetByIdAsync(Guid betId)
        {
            var entity = await _dbContext.Set<BetEntity>().FirstOrDefaultAsync(x => x.BetId == betId);
            return entity == null ? null :
                                    Bet.FromState(
                                        new BetState(betId, 
                                                    entity.MemberId, 
                                                    entity.EndDate, 
                                                    entity.Description, 
                                                    entity.Coins, 
                                                    entity.CreationDate,
                                                    entity.Answers?.Select(x => 
                                                        new AnswerState(x.Member.MemberId,
                                                                        x.IsAccepted,
                                                                        x.DateAnswer))
                                                                   .ToList()));
        }
    }
}
