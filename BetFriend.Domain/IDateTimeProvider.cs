using System;

namespace BetFriend.Domain
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
