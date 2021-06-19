using System;

namespace BetFriend.Domain.Exceptions
{
    public class BetUnknownException : Exception
    {
        public BetUnknownException(string message) : base(message)
        {
        }
    }
}
