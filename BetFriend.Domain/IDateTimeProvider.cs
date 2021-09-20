using System;

namespace BetFriend.Bet.Domain
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
