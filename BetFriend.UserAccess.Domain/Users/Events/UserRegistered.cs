using BetFriend.Shared.Domain;

namespace BetFriend.UserAccess.Domain.Users.Events
{
    public class UserRegistered : IDomainEvent
    {
        private readonly string _id;
        private readonly string _email;

        public UserRegistered(string id, string email)
        {
            _id = id;
            _email = email;
        }

        public override bool Equals(object obj)
        {
            return obj is UserRegistered registered &&
                   _id == registered._id &&
                   _email == registered._email;
        }
    }
}
