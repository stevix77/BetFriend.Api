namespace BetFriend.Bet.Application.Usecases.RetrieveBets
{
    using BetFriend.Bet.Application.Models;
    using BetFriend.Shared.Application.Abstractions.Query;
    using System;
    using System.Collections.Generic;


    public sealed class RetrieveBetsQuery : IQuery<IReadOnlyCollection<BetDto>>
    {
        public RetrieveBetsQuery()
        {
        }
    }
}
