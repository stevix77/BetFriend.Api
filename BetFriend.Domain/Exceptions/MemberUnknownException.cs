namespace BetFriend.Domain.Exceptions
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
