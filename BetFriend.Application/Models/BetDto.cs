namespace BetFriend.Bet.Application.Models
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetDto
    {
        public BetDto(BetState state)
        {
            Id = state.BetId;
            Description = state.Description;
            Creator = new MemberDto(state.Creator);
            Coins = state.Coins;
            EndDate = state.EndDate;
            Participants = state.Answers?.Select(x => new MemberDto() { Id = x.Member.Id.Value, Username = x.Member.Name }).ToList();
        }

        public Guid Id { get; private set; }
        public string Description { get; set; }
        public MemberDto Creator { get; set; }
        public IReadOnlyCollection<MemberDto> Participants { get; set; }
        public int Coins { get; set; }
        public DateTime EndDate { get; set; }
    }
}
