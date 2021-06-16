namespace BetFriend.Infrastructure.DateTimeProvider
{
    using BetFriend.Domain;
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime _dateTime;

        public DateTimeProvider()
        {
            _dateTime = DateTime.UtcNow;
        }

        public DateTime GetDateTime() => _dateTime;
    }
}
