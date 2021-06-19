namespace BetFriend.Domain.Bets
{
    using System;

    public class AnswerState
    {
        public Guid MemberId { get; }
        public bool IsAccepted { get; }
        public DateTime DateAnswer { get; }

        public AnswerState(Guid memberId, bool isAccepted, DateTime dateAnswer)
        {
            MemberId = memberId;
            IsAccepted = isAccepted;
            DateAnswer = dateAnswer;
        }
    }
}
