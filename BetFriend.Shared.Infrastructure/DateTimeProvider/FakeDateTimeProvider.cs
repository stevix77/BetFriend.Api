namespace BetFriend.Shared.Infrastructure.DateTimeProvider
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

        public DateTime Now { get => _dateTime; }
    }
}
