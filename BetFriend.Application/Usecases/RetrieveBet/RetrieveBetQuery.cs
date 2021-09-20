namespace BetFriend.Bet.Application.Usecases.RetrieveBet
{
    using BetFriend.Bet.Application.Abstractions.Query;
    using BetFriend.Bet.Application.Models;
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
