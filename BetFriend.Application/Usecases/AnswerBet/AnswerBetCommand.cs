namespace BetFriend.Application.Usecases.AnswerBet
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Domain;
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
