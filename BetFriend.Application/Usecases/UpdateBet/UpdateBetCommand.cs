namespace BetFriend.Application.Usecases.UpdateBet
{
    using BetFriend.Application.Abstractions.Command;
    using MediatR;
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
