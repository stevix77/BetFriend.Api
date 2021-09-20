namespace BetFriend.Bet.Application.Abstractions.Repository
{
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBetQueryRepository
    {
        Task<IReadOnlyCollection<BetDto>> GetBetsForMemberAsync(Guid memberId);
        Task SaveAsync(BetDto betDto);
        Task<BetDto> GetByIdAsync(Guid betId);
    }
}
