namespace BetFriend.UserAccess.Domain
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
