namespace BetFriend.UserAccess.Domain.Users
{
    using BetFriend.Shared.Domain;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users.Events;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class User : Entity, IAggregateRoot
    {
        private readonly UserId _id;
        private readonly string _username;
        private readonly Email _email;
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
            _email = IsAddressEmailValid(email) ? new(email) : throw new EmailNotValidException();
            _password = password;
            _registerDate = dateTime;
            AddDomainEvent(new UserRegistered(id, email));
        }

        private bool IsAddressEmailValid(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(email);
        }

        public string Username { get => _username; }
        public Email Email { get => _email; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   EqualityComparer<UserId>.Default.Equals(_id, user._id) &&
                   _username == user._username &&
                   _email == user._email;
        }
    }
}
