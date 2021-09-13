namespace BetFriend.Domain.Feeds
{
    using BetFriend.Domain.Bets;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class Feed
    {
        private Feed(Guid feedId)
        {
            Id = feedId;
            _bets = new List<BetState>();
        }

        public Guid Id { get; private set; }
        private ICollection<BetState> _bets;
        public IReadOnlyCollection<BetState> Bets { get => _bets.ToList(); }

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
