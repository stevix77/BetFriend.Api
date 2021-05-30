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
        private readonly MemberId[] _participants;
        private readonly MemberId _creatorId;
        private readonly string _description;

        private Bet(BetId betId, DateTime endDate, MemberId[] participants, MemberId creatorId, string description)
        {
            _creationDate = DateTime.UtcNow;
            _betId = betId;
            _endDate = new EndDate(endDate, _creationDate);
            _participants = participants;
            _creatorId = creatorId;
            _description = description;

            AddDomainEvent(new BetCreated(betId, _creatorId, _participants));

        }

        public static Bet FromState(BetState state)
        {
            return new Bet(new BetId(state.BetId),
                            state.EndDate,
                            state.Participants.Select(x => new MemberId(x))
                                               .ToArray(),
                            new MemberId(state.CreatorId),
                            state.Description);
        }

        public BetState State
        {
            get => new(_betId.Value,
                        _creatorId.Value,
                        _endDate.Value,
                        _description,
                        _participants.Select(x => x.Value)
                                     .ToArray(),
                        _creationDate);
        }


        public static Bet Create(BetId betId, MemberId creatorId, DateTime endDate, string description, MemberId[] participants)
        {
            return new Bet(betId, endDate, participants, creatorId, description);
        }
    }
}
