namespace BetFriend.UserAccess.Domain.Exceptions
{
    using System;

    public class AuthenticationNotValidException : Exception
    {
        public AuthenticationNotValidException(string message) : base(message)
        {
        }
    }
}
