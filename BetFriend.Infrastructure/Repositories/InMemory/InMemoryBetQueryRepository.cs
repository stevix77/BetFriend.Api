namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryBetQueryRepository : IBetQueryRepository
    {
        private readonly IList<BetDto> _bets;

        public InMemoryBetQueryRepository(List<BetDto> bets = null)
        {
            _bets = bets ?? (_bets = new List<BetDto>());
        }

        public async Task<IReadOnlyCollection<BetDto>> GetBetsForMemberAsync(Guid memberId)
        {
            return await Task.FromResult(_bets.Where(x => x.Creator.Id.Equals(memberId)
                                                          || x.Participants.Any(x => x.Id.Equals(memberId)))
                                              .ToList());
        }

        public Task SaveAsync(BetDto betDto)
        {
            var dto = _bets.FirstOrDefault(x => x.Id == betDto.Id);
            if (dto == null)
                _bets.Add(betDto);
            else
            {
                dto.Coins = betDto.Coins;
                dto.Description = betDto.Description;
                dto.EndDate = betDto.EndDate;
                dto.Participants = betDto.Participants;
            }
            return Task.CompletedTask;
        }

        public Task<BetDto> GetByIdAsync(Guid betId)
        {
            return Task.FromResult(_bets.FirstOrDefault(x => x.Id == betId));
        }
    }
}
