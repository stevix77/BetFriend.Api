using System;

namespace BetFriend.Bet.Application.Exceptions
{
    public class BetNotFoundException : Exception
    {
        public BetNotFoundException(string message) : base(message)
        {
        }
    }
}
