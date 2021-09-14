using System;

namespace BetFriend.Application.Exceptions
{
    public class BetNotFoundException : Exception
    {
        public BetNotFoundException(string message) : base(message)
        {
        }
    }
}
