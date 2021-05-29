namespace BetFriend.Application.Usecases.LaunchBet
{
    using System;

    public class LaunchBetCommand
    {
        public LaunchBetCommand(Guid betId, Guid memberId, DateTime endDate, Guid[] participants, string description)
        {
            BetId = betId;
            CreatorId = memberId;
            EndDate = endDate;
            Participants = participants;
            Description = description;
        }

        public Guid BetId { get; }
        public Guid CreatorId { get; }
        public DateTime EndDate { get; }
        public Guid[] Participants { get; }
        public string Description { get; }
    }
}
