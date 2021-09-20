namespace BetFriend.Bet.Domain.Bets
{
    using System;

    public class Answer
    {
        public Answer(bool accepted, DateTime dateAnswer)
        {
            Accepted = accepted;
            DateAnswer = dateAnswer;
        }

        public bool Accepted { get; }
        public DateTime DateAnswer { get; }
    }
}