using System;

namespace BetFriend.Domain.Exceptions
{
    public class AnswerTooLateException : Exception
    {
        public AnswerTooLateException(string message) : base(message)
        {
        }
    }
}
