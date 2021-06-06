namespace BetFriend.Domain.Bets
{
    using BetFriend.Domain.Bets.Events;
    using BetFriend.Domain.Members;
    using System;

    public class Bet : Entity, IAggregateRoot
    {
        private readonly BetId _betId;
        private readonly EndDate _endDate;
        private readonly DateTime _creationDate;
        private readonly int _coins;
        private readonly MemberId _creatorId;
        private readonly string _description;

        private Bet(BetId betId, DateTime endDate, int coins, MemberId creatorId, string description)
        {
            _creationDate = DateTime.UtcNow;
            _betId = betId;
            _endDate = new EndDate(endDate, _creationDate);
            _coins = coins;
            _creatorId = creatorId;
            _description = description;

            AddDomainEvent(new BetCreated(betId, _creatorId));

        }

        private Bet(BetState state)
        {
            _betId = new BetId(state.BetId);
            _endDate = new EndDate(state.EndDate);
            _coins = state.Coins;
            _creatorId = new MemberId(state.CreatorId);
            _description = state.Description;
            _creationDate = state.CreationDate;
        }

        public static Bet FromState(BetState state)
        {
            return new Bet(state);
        }

        public BetState State
        {
            get => new(_betId.Value,
                        _creatorId.Value,
                        _endDate.Value,
                        _description,
                        _coins,
                        _creationDate);
        }


        public static Bet Create(BetId betId, DateTime endDate, string description, int coins, MemberId creatorId)
        {
            return new Bet(betId, endDate, coins, creatorId, description);
        }
    }
}
