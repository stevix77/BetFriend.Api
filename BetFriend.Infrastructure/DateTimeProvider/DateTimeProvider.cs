namespace BetFriend.Infrastructure.DateTimeProvider
{
    using BetFriend.Domain;
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider()
        {
        }

        public DateTime Now { get => DateTime.UtcNow; }
    }
}
