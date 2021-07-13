namespace BetFriend.Application.Abstractions.Repository
{
    using BetFriend.Application.ViewModels;
    using BetFriend.Domain.Bets;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBetQueryRepository
    {
        Task<IReadOnlyCollection<BetViewModel>> GetBetsForMemberAsync(Guid memberId);
        Task SaveAsync(BetViewModel betViewModel);
    }
}
