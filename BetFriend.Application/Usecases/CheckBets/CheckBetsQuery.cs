namespace BetFriend.Application.Usecases.CheckBets
{
    using BetFriend.Application.Abstractions.Query;
    using BetFriend.Application.ViewModels;
    using System;
    using System.Collections.Generic;


    public sealed class CheckBetsQuery : IQuery<IReadOnlyCollection<BetViewModel>>
    {
        public CheckBetsQuery(Guid memberId)
        {
            MemberId = memberId;
        }

        public Guid MemberId { get; }
    }
}
