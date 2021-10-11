namespace BetFriend.Bet.Application.Usecases.LaunchBet
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;

    public class LaunchBetCommand : ICommand
    {
        public LaunchBetCommand(Guid betId, DateTime endDate, int tokens, string description)
        {
            BetId = betId;
            EndDate = endDate;
            Coins = tokens;
            Description = description;
        }

        public Guid BetId { get; }
        public DateTime EndDate { get; }
        public int Coins { get; }
        public string Description { get; }
    }
}
