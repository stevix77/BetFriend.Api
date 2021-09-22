namespace BetFriend.Bet.Application.Usecases.AnswerBet
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Domain;
    using System;

    public class AnswerBetCommand : ICommand
    {
        public AnswerBetCommand(Guid memberId, Guid betId, bool answer, IDateTimeProvider dateAnswer)
        {
            MemberId = memberId;
            BetId = betId;
            IsAccepted = answer;
            DateTimeProvider = dateAnswer;
        }

        public Guid MemberId { get; }
        public Guid BetId { get; }
        public bool IsAccepted { get; }
        public IDateTimeProvider DateTimeProvider { get; }
    }
}
