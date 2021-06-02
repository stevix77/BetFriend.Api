using System;

namespace BetFriend.Domain.Members
{
    public class Member
    {
        private MemberId _creatorId;
        private int _wallet;

        public Member(MemberId creatorId, int wallet)
        {
            _creatorId = creatorId;
            _wallet = wallet;
        }

        public Guid CreatorId { get => _creatorId.Value; }

        internal bool CanBet(int tokens)
        {
            return _wallet >= tokens;
        }
    }
}