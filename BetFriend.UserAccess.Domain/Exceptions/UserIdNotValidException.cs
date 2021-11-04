namespace BetFriend.UserAccess.Domain.Exceptions
{
    using System;

    [Serializable]
    public class UserIdNotValidException : Exception
    {
        public UserIdNotValidException() : base("Userid is empty or not valid")
        {
        }
    }
}
