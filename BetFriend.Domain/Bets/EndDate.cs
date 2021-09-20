using BetFriend.Bet.Domain.Exceptions;
using System;

namespace BetFriend.Bet.Domain.Bets
{
    public class EndDate
    {
        private DateTime _value;

        public EndDate(DateTime value, DateTime creationDate)
        {
            if (value <= creationDate)
                throw new EndDateNotValidException("The end date is before the current date");
            _value = value;
        }

        public EndDate(DateTime value)
        {
            _value = value;
        }

        public DateTime Value { get => _value; }
    }
}
