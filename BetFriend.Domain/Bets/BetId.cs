namespace BetFriend.Domain.Bets
{
    using System;


    public class BetId
    {
        private Guid _value;

        public BetId(Guid value)
        {
            _value = value;
        }

        public Guid Value { get => _value; }
    }
}
