namespace BetFriend.Infrastructure.Repositories.InMemory
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryBetQueryRepository : IBetQueryRepository
    {
        private readonly IList<BetViewModel> _bets;

        public InMemoryBetQueryRepository(List<BetViewModel> bets)
        {
            _bets = bets;
        }

        public async Task<IReadOnlyCollection<BetViewModel>> GetBetsForMemberAsync(Guid memberId)
        {
            return await Task.FromResult(_bets.Where(x => x.Creator.Equals(memberId)
                                                          || x.Participants.Any(x => x.Id.Equals(memberId)))
                                              .ToList()
                                              .AsReadOnly());
        }
    }
}
