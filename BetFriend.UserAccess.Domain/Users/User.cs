using System;
using System.Collections.Generic;

namespace BetFriend.UserAccess.Domain.Users
{
    public class User
    {
        private readonly UserId _id;
        private readonly string _username;
        private readonly string _email;
        private readonly string _password;
        private readonly DateTime _registerDate;

        public User(string id,
                    string username,
                    string email,
                    string password,
                    DateTime dateTime)
        {
            _id = new(id);
            _username = username;
            _email = email;
            _password = password;
            _registerDate = dateTime;
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   EqualityComparer<UserId>.Default.Equals(_id, user._id) &&
                   _username == user._username &&
                   _email == user._email &&
                   _password == user._password &&
                   _registerDate == user._registerDate;
        }
    }

    internal struct UserId
    {
        private string id;

        public UserId(string id)
        {
            this.id = id;
        }
    }
}
