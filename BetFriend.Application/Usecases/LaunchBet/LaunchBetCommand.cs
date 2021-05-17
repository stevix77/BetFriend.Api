namespace BetFriend.Application.Usecases.LaunchBet
{
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
    using System;


    public class LaunchBetCommand
    {
        public LaunchBetCommand(BetId betId, MemberId memberId, DateTime endDate, MemberId[] participants, string description)
        {
            BetId = betId;
            CreatorId = memberId;
            EndDate = endDate;
            Participants = participants;
            Description = description;
        }

        public BetId BetId { get; }
        public MemberId CreatorId { get; }
        public DateTime EndDate { get; }
        public MemberId[] Participants { get; }
        public string Description { get; }
    }
}
