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

        private bool CanBet(int tokens)
        {
            return _wallet >= tokens;
        }

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int tokens)
        {
            if(!CanBet(tokens))
                throw new MemberDoesNotEnoughTokensException();

            return Bet.Create(betId, endDate, description, tokens, _memberId);
        }
    }
}