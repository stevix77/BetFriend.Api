using System;

namespace BetFriend.Shared.Domain
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
