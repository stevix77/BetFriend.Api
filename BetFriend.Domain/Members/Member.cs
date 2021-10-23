namespace BetFriend.Bet.Domain.Members
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members.Events;
    using BetFriend.Bet.Domain.Subscriptions;
    using BetFriend.Shared.Domain;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;


    public class Member : Entity
    {
        private readonly List<Subscription> _subscriptions;

        public Member(MemberId creatorId, string memberName, decimal wallet)
        {
            Id = creatorId;
            Name = memberName;
            Wallet = wallet;
            _subscriptions = new List<Subscription>();
        }

        public static Member Create(MemberId creatorId, string memberName, decimal wallet)
        {
            var member = new Member(creatorId, memberName, wallet);
            member.AddDomainEvent(new MemberCreated(creatorId.Value));
            return member;
        }

        public MemberId Id { get; }
        public decimal Wallet { get; private set; }
        public string Name { get; }
        public IReadOnlyCollection<Subscription> Subscriptions { get => _subscriptions; }

        private bool CanBet(int coins)
        {
            return Wallet >= coins;
        }

        public Bet CreateBet(BetId betId, DateTime endDate, string description, int coins, DateTime creationDate)
        {
            if (!CanBet(coins))
                throw new MemberHasNotEnoughCoinsException(Wallet, coins);

            return Bet.Create(betId, endDate, description, coins, this, creationDate);
        }

        public void Subscribe(Subscription subscription)
        {
            if (!_subscriptions.Any(x => x.MemberId.Equals(subscription.MemberId)))
            {
                _subscriptions.Add(subscription);
                AddDomainEvent(new MemberSubscribed(Id, subscription.MemberId));
            }
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
            if (bet.IsSuccess())
                Wallet -= (decimal)coins / bet.State.Answers.Count;
            else
                Wallet += (decimal)coins / bet.State.Answers.Count;
        }

        private void CheckAnswer(Bet bet, DateTime dateAnswer)
        {
            if (Wallet < bet.GetCoins())
                throw new MemberHasNotEnoughCoinsException(Wallet, bet.GetCoins());

            if (dateAnswer.CompareTo(bet.GetEndDateToAnswer()) > 0)
                throw new AnswerTooLateException($"The date limit to answer was at : {bet.GetEndDateToAnswer().ToLongDateString()}");
        }
    }
}