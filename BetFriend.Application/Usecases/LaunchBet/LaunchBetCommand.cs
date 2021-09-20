namespace BetFriend.Bet.Application.Usecases.LaunchBet
{
    using BetFriend.Bet.Application.Abstractions.Command;
    using BetFriend.Bet.Domain;
    using System;

    public class LaunchBetCommand : ICommand
    {
        public LaunchBetCommand(Guid betId, Guid memberId, DateTime endDate, int tokens, string description, IDateTimeProvider creationDate)
        {
            BetId = betId;
            CreatorId = memberId;
            EndDate = endDate;
            Coins = tokens;
            Description = description;
            CreationDate = creationDate;
        }

        public Guid BetId { get; }
        public Guid CreatorId { get; }
        public DateTime EndDate { get; }
        public int Coins { get; }
        public string Description { get; }
        public IDateTimeProvider CreationDate { get; }
    }
}
