namespace BetFriend.Bet.Application.Usecases.RetrieveMember
{
    using BetFriend.Bet.Application.Models;
    using BetFriend.Shared.Application.Abstractions.Query;
    using System;


    public class RetrieveMemberQuery : IQuery<MemberDto>
    {
        public RetrieveMemberQuery(Guid memberId)
        {
            MemberId = memberId;
        }

        public Guid MemberId { get; }
    }
}