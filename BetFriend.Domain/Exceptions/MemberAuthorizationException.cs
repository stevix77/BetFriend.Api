namespace BetFriend.Bet.Domain.Exceptions
{
    using System;


    public class MemberAuthorizationException : Exception
    {
        public MemberAuthorizationException(string message) : base(message)
        {
        }
    }
}
