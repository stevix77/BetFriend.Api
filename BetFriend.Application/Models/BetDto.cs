namespace BetFriend.Application.Models
{
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetDto
    {
        public BetDto(BetState state, Member member)
        {
            Id = state.BetId;
            Description = state.Description;
            CreatorId = state.CreatorId;
            Coins = state.Coins;
            EndDate = state.EndDate;
            CreatorUsername = "";
            Participants = state.Answers?.Select(x => new MemberDto() { Id = x.MemberId }).ToList();
        }

        public Guid Id { get; private set; }
        public string Description { get; set; }
        public Guid CreatorId { get; set; }
        public IReadOnlyCollection<MemberDto> Participants { get; set; }
        public int Coins { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatorUsername { get; private set; }
    }
}
