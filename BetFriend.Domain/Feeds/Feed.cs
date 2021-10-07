namespace BetFriend.Bet.Domain.Feeds
{
    using BetFriend.Bet.Domain.Bets;
    using System;
    using System.Collections.Generic;


    public class Feed
    {
        private Feed(Guid feedId)
        {
            Id = feedId;
            _bets = new List<BetState>();
        }

        public Guid Id { get; private set; }
        private readonly List<BetState> _bets;
        public IReadOnlyCollection<BetState> Bets { get => _bets; }

        public static Feed Create(Guid feedId)
        {
            return new Feed(feedId);
        }

        public void AddBet(BetState bet)
        {
            _bets.Add(bet);
        }
    }
}
