using System;

namespace BetFriend.Bet.Domain.Exceptions
{
    public class BetUnknownException : Exception
    {
        public BetUnknownException(string message) : base(message)
        {
        }
    }
}
