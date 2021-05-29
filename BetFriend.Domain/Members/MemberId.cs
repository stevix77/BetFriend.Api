namespace BetFriend.Domain.Members
{
    using System;

    public class MemberId
    {
        private Guid _value;
        public MemberId(Guid value)
        {
            _value = value;
        }

        public Guid Value { get => _value; }
    }
}
