﻿using BetFriend.UserAccess.Domain.Exceptions;

namespace BetFriend.UserAccess.Domain.Users
{
    public struct UserId
    {
        private string id;

        public UserId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new UserIdNotValidException();
            this.id = id;
        }
    }
}