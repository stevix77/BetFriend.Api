namespace BetFriend.Domain.Bets
{
    using BetFriend.Domain.Bets.Events;
    using BetFriend.Domain.Members;
    using System;
    using System.Linq;

    public class Bet : Entity, IAggregateRoot
    {
        private readonly BetId _betId;
        private readonly EndDate _endDate;
        private readonly DateTime _creationDate;
        private readonly int _tokens;
        private readonly MemberId _creatorId;
        private readonly string _description;

        private Bet(BetId betId, DateTime endDate, int tokens, MemberId creatorId, string description)
        {
            _creationDate = DateTime.UtcNow;
            _betId = betId;
            _endDate = new EndDate(endDate, _creationDate);
            _tokens = tokens;
            _creatorId = creatorId;
            _description = description;

            AddDomainEvent(new BetCreated(betId, _creatorId, null));

        }

        public static Bet FromState(BetState state)
        {
            return new Bet(new BetId(state.BetId),
                            state.EndDate,
                            state.Tokens,
                            new MemberId(state.CreatorId),
                            state.Description);
        }

        public BetState State
        {
            get => new(_betId.Value,
                        _creatorId.Value,
                        _endDate.Value,
                        _description,
                        _tokens,
                        _creationDate);
        }


        public static Bet Create(BetId betId, MemberId creatorId, DateTime endDate, string description, int tokens)
        {
            return new Bet(betId, endDate, tokens, creatorId, description);
        }
    }
}
