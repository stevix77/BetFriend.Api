using System;

namespace BetFriend.Bet.Domain.Exceptions
{
    public class AnswerTooLateException : Exception
    {
        public AnswerTooLateException(string message) : base(message)
        {
        }
    }
}
