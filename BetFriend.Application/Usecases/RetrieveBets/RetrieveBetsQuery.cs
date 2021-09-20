namespace BetFriend.Bet.Application.Usecases.RetrieveBets
{
    using BetFriend.Bet.Application.Abstractions.Query;
    using BetFriend.Bet.Application.Models;
    using System;
    using System.Collections.Generic;


    public sealed class RetrieveBetsQuery : IQuery<IReadOnlyCollection<BetDto>>
    {
        public RetrieveBetsQuery(Guid memberId)
        {
            MemberId = memberId;
        }

        public Guid MemberId { get; }
    }
}
