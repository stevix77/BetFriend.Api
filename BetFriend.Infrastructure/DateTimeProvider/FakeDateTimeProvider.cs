namespace BetFriend.Infrastructure.DateTimeProvider
{
    using BetFriend.Domain;
    using System;


    public class FakeDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime _dateTime;

        public FakeDateTimeProvider(DateTime dateTime)
        {
            this._dateTime = dateTime;
        }

        public DateTime GetDateTime() => _dateTime;
    }
}
