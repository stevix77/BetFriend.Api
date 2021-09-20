namespace BetFriend.Bet.Infrastructure.DateTimeProvider
{
    using BetFriend.Bet.Domain;
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
