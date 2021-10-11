namespace BetFriend.Bet.Application.Usecases.AnswerBet
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;

    public class AnswerBetCommand : ICommand
    {
        public AnswerBetCommand(Guid betId, bool answer)
        {
            BetId = betId;
            IsAccepted = answer;
        }

        public Guid BetId { get; }
        public bool IsAccepted { get; }
    }
}
