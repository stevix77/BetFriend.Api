namespace BetFriend.Domain.Bets
{
    using BetFriend.Domain.Bets.Events;
    using BetFriend.Domain.Members;
    using System.Collections.Generic;
    using System.Linq;

    public class Bet
    {
        private readonly BetId _betId;
        private readonly EndDate _endDate;
        private readonly MemberId[] _participants;
        private readonly MemberId _creatorId;
        private readonly string _description;

        private Bet(BetId betId, EndDate endDate, MemberId[] participants, MemberId creatorId, string description)
        {
            _betId = betId;
            _endDate = endDate;
            _participants = participants;
            _creatorId = creatorId;
            _description = description;

            DomainEvents = new List<object>();
            DomainEvents.Add(new BetCreated(betId, _creatorId, _participants));
        }

        public static Bet FromState(BetState state)
        {
            return new Bet(new BetId(state.BetId),
                            new EndDate(state.EndDate),
                            state.Participants.Select(x => new MemberId(x))
                                               .ToArray(),
                            new MemberId(state.MemberId),
                            state.Description);
        }

        public BetState State
        {
            get => new(_betId.Value,
                        _creatorId.Value,
                        _endDate.Value,
                        _description,
                        _participants.Select(x => x.Value)
                                     .ToArray());
        }
        public List<object> DomainEvents { get; set; }

        public static Bet Create(BetId betId, MemberId creatorId, EndDate endDate, string description, MemberId[] participants)
        {
            return new Bet(betId, endDate, participants, creatorId, description);
        }
    }
}
