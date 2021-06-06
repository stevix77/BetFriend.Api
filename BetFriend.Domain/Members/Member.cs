using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using System;

namespace BetFriend.Domain.Members
{
    public class Member
    {
        private MemberId _memberId;
        private int _wallet;

        public Member(MemberId creatorId, int wallet)
        {
            _memberId = creatorId;
            _wallet = wallet;
        }

        public Guid MemberId { get => _memberId.Value; }

        private bool CanBet(int coins)
        {
            return _wallet >= coins;
        }

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int coins)
        {
            if(!CanBet(coins))
                throw new MemberDoesNotEnoughCoinsException();

            return Bet.Create(betId, endDate, description, coins, _memberId);
        }
    }
}