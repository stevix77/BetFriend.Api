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
        private readonly Member _creator;
        private readonly string _description;
        private readonly Dictionary<MemberId, Answer> _answers;
        private Status _status;

        private Bet(BetId betId, DateTime endDate, int coins, Member creator, string description, DateTime creationDate)
        {
            _creationDate = creationDate;
            _betId = betId;
            _endDate = new EndDate(endDate, _creationDate);
            _coins = coins;
            _creator = creator;
            _description = description;
            _answers = new Dictionary<MemberId, Answer>();

            AddDomainEvent(new BetCreated(betId, _creator.Id));

        }

        public void Close(bool success, IDateTimeProvider dateTimeProvider)
        {
            _status = new Status(success, dateTimeProvider.Now);
            AddDomainEvent(new BetClosed(_betId.Value));
        }

        private Bet(BetState state)
        {
            _betId = new BetId(state.BetId);
            _endDate = new EndDate(state.EndDate);
            _coins = state.Coins;
            _creator = new Member(state.Creator.Id,
                                  state.Creator.Name,
                                  state.Creator.Wallet);
            _description = state.Description;
            _creationDate = state.CreationDate;
            _answers = new Dictionary<MemberId, Answer>(
                        state.Answers.Select(x => 
                            new KeyValuePair<MemberId, Answer>(
                                new MemberId(x.MemberId), 
                                new Answer(x.IsAccepted, x.DateAnswer)
                            )
                        ));
            _status = state.Status;
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
                        _creator,
                        _endDate.Value,
                        _description,
                        _coins,
                        _creationDate,
                        _answers.Select(x => new AnswerState(x.Key.Value, x.Value.Accepted, x.Value.DateAnswer))
                                .ToList()
                                .AsReadOnly(),
                        _status);
        }

        public static Bet Create(BetId betId, DateTime endDate, string description, int coins, Member creator, DateTime creationDate)
        {
            return new Bet(betId, endDate, coins, creator, description, creationDate);
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
    public class BetClosed : IDomainEvent
    {
        public BetClosed(Guid betId)
        {
            BetId = betId;
        }

        public Guid BetId { get; }
    }
}
