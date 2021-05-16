namespace BetFriend.Application.Usecases.LaunchBet
{
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
    using System;


    public class LaunchBetCommand
    {
        public LaunchBetCommand(BetId betId, MemberId memberId, DateTime endDate, MemberId[] participants)
        {
            BetId = betId;
            MemberId = memberId;
            EndDate = endDate;
            Participants = participants;
        }

        public BetId BetId { get; }
        public MemberId MemberId { get; }
        public DateTime EndDate { get; }
        public MemberId[] Participants { get; }
    }
}
