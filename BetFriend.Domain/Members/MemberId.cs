namespace BetFriend.Bet.Domain.Members
{
    using System;

    public struct MemberId
    {
        private Guid _value;
        public MemberId(Guid value)
        {
            _value = value;
        }

        public Guid Value { get => _value; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
