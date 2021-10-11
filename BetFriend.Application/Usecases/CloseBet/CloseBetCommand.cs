namespace BetFriend.Bet.Application.Usecases.CloseBet
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;


    public sealed class CloseBetCommand : ICommand
    {
        public Guid BetId { get; }

        public bool Success { get; }

        public CloseBetCommand(Guid betId, bool success)
        {
            BetId = betId;
            Success = success;
        }
    }
}
