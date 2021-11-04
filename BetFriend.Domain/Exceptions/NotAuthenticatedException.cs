namespace BetFriend.Bet.Domain.Exceptions
{
    using System;

    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException() : base("The user is not logged")
        {
        }
    }
}
