namespace BetFriend.UserAccess.Domain.Exceptions
{
    using System;
    [Serializable]
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException()
        {
        }
    }
}
