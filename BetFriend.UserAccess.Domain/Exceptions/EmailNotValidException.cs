namespace BetFriend.UserAccess.Domain.Exceptions
{
    using System;
    [Serializable]
    public class EmailNotValidException : Exception
    {
        public EmailNotValidException(string message) : base(message)
        {
        }
    }
}
