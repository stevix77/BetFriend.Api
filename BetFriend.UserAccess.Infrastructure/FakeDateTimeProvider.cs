namespace BetFriend.UserAccess.Infrastructure
{
    using BetFriend.Shared.Domain;
    using System;

    public class FakeDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime _dateTime;

        public FakeDateTimeProvider(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public DateTime Now => _dateTime;
    }
}
