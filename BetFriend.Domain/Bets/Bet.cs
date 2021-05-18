namespace BetFriend.Domain.Bets
{
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using System;

    public class Bet
    {
        private readonly BetId _betId;
        private readonly DateTime _endDate;
        private readonly MemberId[] _participants;
        private readonly MemberId _creatorId;
        private readonly string _description;

        private Bet(BetId betId, DateTime endDate, MemberId[] participants, MemberId creatorId, string description)
        {
            if (endDate <= DateTime.UtcNow)
                throw new EndDateNotValidException("The end date is before the current date");

            _betId = betId;
            _endDate = endDate;
            _participants = participants;
            _creatorId = creatorId;
            _description = description;
        }

        public static Bet FromState(BetState bet)
        {
            return new Bet(bet.BetId, bet.EndDate, bet.Participants, bet.MemberId, bet.Description);
        }

        public BetState State { get => new (_betId, _creatorId, _endDate, _description, _participants); }

        public static Bet Create(BetId betId, MemberId creatorId, DateTime endDate, string description, MemberId[] participants)
        {
            return new Bet(betId, endDate, participants, creatorId, description);
        }

        public class BetState
        {
            public BetState(BetId betId, MemberId memberId, DateTime endDate, string description, MemberId[] participants)
            {
                BetId = betId;
                MemberId = memberId;
                EndDate = endDate;
                Description = description;
                Participants = participants;
            }

            public BetId BetId { get; }
            public MemberId MemberId { get; }
            public DateTime EndDate { get; }
            public string Description { get; }
            public MemberId[] Participants { get; }

            public override bool Equals(object obj)
            {
                return ((BetState)obj).BetId == BetId;
            }
        }
    }
}
