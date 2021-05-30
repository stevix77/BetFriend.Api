namespace BetFriend.Domain.Bets
{
    using System;


    public class BetId
    {
        private Guid _value;

        public BetId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("BetId should be initialized");

            _value = value;
        }

        public Guid Value { get => _value; }
    }
}
