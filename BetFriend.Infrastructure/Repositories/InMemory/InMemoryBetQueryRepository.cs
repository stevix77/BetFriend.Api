namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
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
            return await Task.FromResult(_bets.Where(x => x.CreatorId.Equals(memberId)
                                                          || x.Participants.Any(x => x.Id.Equals(memberId)))
                                              .ToList());
        }

        public Task SaveAsync(BetDto betViewModel)
        {
            _bets.Add(betViewModel);
            return Task.CompletedTask;
        }

        public Task SaveAsync(BetState state, Member member)
        {
            var betVM = new BetDto
            {
                Coins = state.Coins,
                CreatorId = member.MemberId.Value,
                CreatorUsername = member.MemberName,
                Description = state.Description,
                EndDate = state.EndDate,
                Id = state.BetId
            };
            _bets.Add(betVM);
            return Task.CompletedTask;
        }

        public Task<BetDto> GetByIdAsync(Guid betId)
        {
            return Task.FromResult(_bets.FirstOrDefault(x => x.Id == betId));
        }
    }
}
