namespace BetFriend.Bet.Domain.Bets
{
    using BetFriend.Bet.Domain.Members;
    using System;

    public class AnswerState
    {
        public Member Member { get; }
        public bool IsAccepted { get; }
        public DateTime DateAnswer { get; }

        public AnswerState(Member member, bool isAccepted, DateTime dateAnswer)
        {
            Member = member;
            IsAccepted = isAccepted;
            DateAnswer = dateAnswer;
        }
    }
}
