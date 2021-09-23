using BetFriend.UserAccess.Domain.Exceptions;
using System;

namespace BetFriend.UserAccess.Domain.Users
{
    public struct Email
    {
        private readonly string _email;

        public Email(string email)
        {
            _email = email;
        }

        public override bool Equals(object obj)
        {
            return obj is Email email &&
                   _email == email._email;
        }

        public override string ToString()
        {
            return _email;
        }

        public static bool operator ==(Email left, Email right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !(left == right);
        }
    }
}
