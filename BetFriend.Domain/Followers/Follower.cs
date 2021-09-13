using BetFriend.Domain.Members;
using System;

namespace BetFriend.Domain.Followers
{
    public class Follower
    {
        public MemberId MemberId { get; }

        public Follower(MemberId memberId)
        {
            MemberId = memberId;
        }
    }
}
