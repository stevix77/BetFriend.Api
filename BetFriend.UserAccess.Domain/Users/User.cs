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
            _id = new UserId(id);
            _username = username;
            _email = IsAddressEmailValid(email) ? new Email(email) : throw new EmailNotValidException($"The address {email} is not a valid address email");
            _password = password;
            _registerDate = dateTime;
            AddDomainEvent(new UserRegistered(id, email, username));
        }

        private User(UserId userId, string username, Email email, string password, DateTime registerDate)
        {
            _id = userId;
            _username = username;
            _email = email;
            _password = password;
            _registerDate = registerDate;
        }

        public static User Restore(string userId, string username, string email, string password, DateTime registerDate)
        {
            return new User(
                new UserId(userId),
                username,
                new Email(email),
                password,
                registerDate);
        }

        private static bool IsAddressEmailValid(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(email);
        }

        public string Username { get => _username; }
        public string Email { get => _email.ToString(); }
        public string Password { get => _password; }
        public string UserId { get => _id.ToString(); }
        public DateTime RegisterDate { get => _registerDate; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   EqualityComparer<UserId>.Default.Equals(_id, user._id) &&
                   _username == user._username &&
                   _email.Equals(user._email);
        }
    }
}
