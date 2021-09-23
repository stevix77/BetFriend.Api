using BetFriend.UserAccess.Domain.Exceptions;

namespace BetFriend.UserAccess.Domain.Users
{
    public struct UserId
    {
        private readonly string _id;

        public UserId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new UserIdNotValidException();
            _id = id;
        }
    }
}
