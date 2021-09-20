namespace BetFriend.Bet.Infrastructure.DateTimeProvider
{
    using BetFriend.Bet.Domain;
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider()
        {
        }

        public DateTime Now { get => DateTime.UtcNow; }
    }
}
