namespace BetFriend.Shared.Infrastructure.DateTimeProvider
{
    using BetFriend.Shared.Domain;
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider()
        {
        }

        public DateTime Now { get => DateTime.UtcNow; }
    }
}
