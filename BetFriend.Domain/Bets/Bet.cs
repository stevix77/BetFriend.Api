namespace BetFriend.Domain.Bets
{
    using BetFriend.Domain.Bets.Events;
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Bet : Entity, IAggregateRoot
    {
        private readonly BetId _betId;
        private readonly EndDate _endDate;
        private readonly DateTime _creationDate;
        private readonly int _coins;
        private readonly MemberId _creatorId;
        private readonly string _description;
        private readonly Dictionary<MemberId, Answer> _answers;

        private Bet(BetId betId, DateTime endDate, int coins, MemberId creatorId, string description, DateTime creationDate)
        {
            _creationDate = creationDate;
            _betId = betId;
            _endDate = new EndDate(endDate, _creationDate);
            _coins = coins;
            _creatorId = creatorId;
            _description = description;
            _answers = new Dictionary<MemberId, Answer>();

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
            _answers = new Dictionary<MemberId, Answer>(
                        state.Answers.Select(x => 
                            new KeyValuePair<MemberId, Answer>(
                                new MemberId(x.MemberId), 
                                new Answer(x.IsAccepted, x.DateAnswer)
                            )
                        ));
        }

        internal int GetCoins() => _coins;

        internal void AddAnswer(MemberId memberId, bool isAccepted, DateTime dateAnswer)
        {
            _answers.Add(memberId, new Answer(isAccepted, dateAnswer));
            AddDomainEvent(new BetAnswered(_betId.Value, memberId.Value, isAccepted));
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
                        _creationDate,
                        _answers.Select(x => new AnswerState(x.Key.Value, x.Value.Accepted, x.Value.DateAnswer))
                                .ToList()
                                .AsReadOnly());
        }


        public static Bet Create(BetId betId, DateTime endDate, string description, int coins, MemberId creatorId, DateTime creationDate)
        {
            return new Bet(betId, endDate, coins, creatorId, description, creationDate);
        }

        public DateTime GetEndDateToAnswer()
        {
            return _creationDate.AddSeconds(_endDate.Value.Subtract(_creationDate).TotalSeconds / 4);
        }

        public Answer GetAnswerForMember(MemberId memberId)
        {
            return _answers.FirstOrDefault(x => x.Key.Value == memberId.Value).Value;
        }
    }
}
