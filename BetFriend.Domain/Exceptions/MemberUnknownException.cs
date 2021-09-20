namespace BetFriend.Bet.Domain.Exceptions
{
    using System;

    [Serializable]
    public class MemberUnknownException : Exception
    {

        public MemberUnknownException(string message) : base(message)
        {
        }
    }
}
