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
            Id = creatorId;
            Name = memberName;
            Wallet = wallet;
            _followers = new List<Follower>();
        }

        public MemberId Id { get; }
        public int Wallet { get; private set; }
        public string Name { get; }
        public IReadOnlyCollection<Follower> Followers { get => _followers.ToList(); }

        private bool CanBet(int coins)
        {
            return Wallet >= coins;
        }

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int coins, DateTime creationDate)
        {
            if(!CanBet(coins))
                throw new MemberHasNotEnoughCoinsException(Wallet, coins);

            return Bet.Create(betId, endDate, description, coins, this, creationDate);
        }

        public void AddFollower(Follower follower)
        {
            _followers.Add(follower);
        }

        public void Answer(Bet bet, bool isAccepted, DateTime dateAnswer)
        {
            CheckAnswer(bet, dateAnswer);

            bet.AddAnswer(this, isAccepted, dateAnswer);
        }

        public void UpdateCreatorWallet(Bet bet)
        {
            var coins = bet.State.Coins;
            if (bet.IsSuccess())
                Wallet += coins;
            else
                Wallet -= coins;
        }

        public void UpdateParticipantWallet(Bet bet)
        {
            var coins = bet.State.Coins;
            if(bet.IsSuccess())
                Wallet -= coins / bet.State.Answers.Count;
            else
                Wallet += coins / bet.State.Answers.Count;
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