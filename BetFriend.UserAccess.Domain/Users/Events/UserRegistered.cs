using BetFriend.Shared.Domain;

namespace BetFriend.UserAccess.Domain.Users.Events
{
    public class UserRegistered : IDomainEvent
    {
        private readonly string _id;
        private readonly string _email;
        private readonly string _username;

        public UserRegistered(string id, string email, string username)
        {
            _id = id;
            _email = email;
            _username = username;
        }

        public string Id
        {
            get => _id;
        }

        public string Email
        {
            get => _email;
        }

        public string Username
        {
            get => _username;
        }

        public override bool Equals(object obj)
        {
            return obj is UserRegistered registered &&
                   _id == registered._id &&
                   _email == registered._email &&
                   _username == registered._username;
        }
    }
}
