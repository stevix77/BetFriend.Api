namespace BetFriend.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EndDateNotValidException : Exception
    {

        public EndDateNotValidException(string message) : base(message)
        {
        }
    }
}
