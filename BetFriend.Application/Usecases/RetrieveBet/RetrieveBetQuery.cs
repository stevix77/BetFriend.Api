namespace BetFriend.Bet.Application.Usecases.RetrieveBet
{
    using BetFriend.Bet.Application.Models;
    using BetFriend.Shared.Application.Abstractions.Query;
    using System;


    public class RetrieveBetQuery : IQuery<BetDto>
    {
        public RetrieveBetQuery(Guid betId)
        {
            BetId = betId;
        }

        public Guid BetId { get; }
    }
}
