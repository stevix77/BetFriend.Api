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
        public int Wallet { get => _wallet; }

        private bool CanBet(int coins)
        {
            return _wallet >= coins;
        }

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int coins, DateTime creationDate)
        {
            if(!CanBet(coins))
                throw new MemberHasNotEnoughCoinsException(_wallet, coins);

            return Bet.Create(betId, endDate, description, coins, _memberId, creationDate);
        }

        public void Answer(Bet bet, bool isAccepted, IDateTimeProvider dateAnswer)
        {
            CheckAnswer(bet, dateAnswer.GetDateTime());

            bet.AddAnswer(_memberId, isAccepted, dateAnswer);
        }

        private void CheckAnswer(Bet bet, DateTime dateAnswer)
        {
            if (_wallet < bet.GetCoins())
                throw new MemberHasNotEnoughCoinsException(_wallet, bet.GetCoins());

            if(dateAnswer.CompareTo(bet.GetEndDateToAnswer()) > 0)
                throw new AnswerTooLateException($"The date limit to answer was at : {bet.GetEndDateToAnswer().ToLongDateString()}");
        }
    }
}