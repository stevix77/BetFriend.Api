namespace BetFriend.Application.Usecases.RetrieveBet
{
    using BetFriend.Application.Abstractions.Query;
    using BetFriend.Application.Models;
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
