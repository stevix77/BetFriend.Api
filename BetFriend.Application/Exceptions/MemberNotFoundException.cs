using System;

namespace BetFriend.Bet.Application.Exceptions
{
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string message) : base(message)
        {
        }
    }
}
