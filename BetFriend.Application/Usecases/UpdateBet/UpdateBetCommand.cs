namespace BetFriend.Bet.Application.Usecases.UpdateBet
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;

    public class UpdateBetCommand : ICommand
    {
        public Guid BetId { get; }

        public UpdateBetCommand(Guid betId)
        {
            BetId = betId;
        }
    }
}
