namespace BetFriend.UserAccess.Domain.Exceptions
{
    using System;
    [Serializable]
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
        {
        }
    }
}
