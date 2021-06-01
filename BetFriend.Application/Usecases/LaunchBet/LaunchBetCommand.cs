namespace BetFriend.Application.Usecases.LaunchBet
{
    using System;

    public class LaunchBetCommand
    {
        public LaunchBetCommand(Guid betId, Guid memberId, DateTime endDate, int tokens, string description)
        {
            BetId = betId;
            CreatorId = memberId;
            EndDate = endDate;
            Tokens = tokens;
            Description = description;
        }

        public Guid BetId { get; }
        public Guid CreatorId { get; }
        public DateTime EndDate { get; }
        public int Tokens { get; }
        public string Description { get; }
    }
}
