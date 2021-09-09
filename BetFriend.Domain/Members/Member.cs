using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using System;

namespace BetFriend.Domain.Members
{
    public class Member
    {

        public Member(MemberId creatorId, string memberName, int wallet)
        {
            MemberId = creatorId;
            MemberName = memberName;
            Wallet = wallet;
        }

        public MemberId MemberId { get; }
        public int Wallet { get; private set; }
        public string MemberName { get; }

        private bool CanBet(int coins)
        {
            return Wallet >= coins;
        }

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int coins, DateTime creationDate)
        {
            if(!CanBet(coins))
                throw new MemberHasNotEnoughCoinsException(Wallet, coins);

            return Bet.Create(betId, endDate, description, coins, MemberId, creationDate);
        }

        public void Answer(Bet bet, bool isAccepted, DateTime dateAnswer)
        {
            CheckAnswer(bet, dateAnswer);

            bet.AddAnswer(MemberId, isAccepted, dateAnswer);
        }

        private void CheckAnswer(Bet bet, DateTime dateAnswer)
        {
            if (Wallet < bet.GetCoins())
                throw new MemberHasNotEnoughCoinsException(Wallet, bet.GetCoins());

            if(dateAnswer.CompareTo(bet.GetEndDateToAnswer()) > 0)
                throw new AnswerTooLateException($"The date limit to answer was at : {bet.GetEndDateToAnswer().ToLongDateString()}");
        }
    }
}