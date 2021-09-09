namespace BetFriend.Application.Usecases.RetrieveBets
{
    using BetFriend.Application.Abstractions.Query;
    using BetFriend.Application.Models;
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
