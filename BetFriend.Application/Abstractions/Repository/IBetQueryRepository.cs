namespace BetFriend.Application.Abstractions.Repository
{
    using BetFriend.Application.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBetQueryRepository
    {
        Task<IReadOnlyCollection<BetViewModel>> GetBetsForMemberAsync(Guid memberId);
    }
}
