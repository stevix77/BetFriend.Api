namespace BetFriend.Bet.Domain.Exceptions
{
    using System;


    [Serializable]
    public class MemberAlreadyExistsException : Exception
    {
        public MemberAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
