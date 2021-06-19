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

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int coins, DateTime creationDate)
        {
            if(!CanBet(coins))
                throw new MemberDoesNotEnoughCoinsException();

            return Bet.Create(betId, endDate, description, coins, _memberId, creationDate);
        }

        public void Answer(Bet bet, bool isAccepted, IDateTimeProvider dateAnswer)
        {
            if (!CanAnswer(dateAnswer.GetDateTime(), bet.GetEndDateToAnswer()))
                throw new AnswerTooLateException($"The date limit to answer was at : {bet.GetEndDateToAnswer().ToLongDateString()}");
            bet.AddAnswer(_memberId, isAccepted, dateAnswer);
        }

        private static bool CanAnswer(DateTime dateAnswer, DateTime endDateToAnswer)
        {
            return dateAnswer.CompareTo(endDateToAnswer) < 0;
        }
    }
}