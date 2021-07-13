namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.ViewModels;
    using BetFriend.Domain.Bets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryBetQueryRepository : IBetQueryRepository
    {
        private readonly IList<BetViewModel> _bets;

        public InMemoryBetQueryRepository(List<BetViewModel> bets = null)
        {
            _bets = bets ?? (_bets = new List<BetViewModel>());
        }

        public async Task<IReadOnlyCollection<BetViewModel>> GetBetsForMemberAsync(Guid memberId)
        {
            return await Task.FromResult(_bets.Where(x => x.CreatorId.Equals(memberId)
                                                          || x.Participants.Any(x => x.Id.Equals(memberId)))
                                              .ToList());
        }

        public Task SaveAsync(BetViewModel betViewModel)
        {
            _bets.Add(betViewModel);
            return Task.CompletedTask;
        }
    }
}
