using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Followers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetFriend.Domain.Members
{
    public class Member
    {
        private readonly ICollection<Follower> _followers;

        public Member(MemberId creatorId, string memberName, int wallet)
        {
            MemberId = creatorId;
            MemberName = memberName;
            Wallet = wallet;
            _followers = new List<Follower>();
        }

        public MemberId MemberId { get; }
        public int Wallet { get; private set; }
        public string MemberName { get; }
        public IReadOnlyCollection<Follower> Followers { get => _followers.ToList(); }

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

        public void AddFollower(Follower follower)
        {
            _followers.Add(follower);
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