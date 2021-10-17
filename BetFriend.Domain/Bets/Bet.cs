namespace BetFriend.Bet.Domain.Bets
{
    using BetFriend.Bet.Domain.Bets.Events;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Bet : Entity, IAggregateRoot
    {
        private readonly BetId _betId;
        private readonly EndDate _endDate;
        private readonly DateTime _creationDate;
        private readonly int _coins;
        private readonly Member _creator;
        private readonly string _description;
        private readonly Dictionary<Member, Answer> _answers;
        private Status _status;

        private Bet(BetId betId, DateTime endDate, int coins, Member creator, string description, DateTime creationDate)
        {
            _creationDate = creationDate;
            _betId = betId;
            _endDate = new EndDate(endDate, _creationDate);
            _coins = coins;
            _creator = creator;
            _description = description;
            _answers = new Dictionary<Member, Answer>();

            AddDomainEvent(new BetCreated(betId, _creator.Id));

        }

        private Bet(BetState state)
        {
            _betId = new BetId(state.BetId);
            _endDate = new EndDate(state.EndDate);
            _coins = state.Coins;
            _creator = state.Creator;
            _description = state.Description;
            _creationDate = state.CreationDate;
            _answers = new Dictionary<Member, Answer>(
                        state.Answers?.Select(x =>
                            new KeyValuePair<Member, Answer>(
                                x.Member,
                                new Answer(x.IsAccepted, x.DateAnswer)
                            )
                        ));
            _status = state.Status;
        }

        public IReadOnlyCollection<Member> GetAllMembers()
        {
            var members = _answers.Select(x => x.Key).ToList();
            members.Add(_creator);
            return members;
        }

        public void Close(MemberId memberId, bool success, IDateTimeProvider dateTimeProvider)
        {
            if(_creator.Id.Value != memberId.Value)
                throw new MemberAuthorizationException($"Member {memberId.Value} is not creator of this bet");
            _status = new Status(success, dateTimeProvider.Now);
            AddDomainEvent(new BetClosed(_betId.Value));
        }

        public void UpdateWallets()
        {
            _creator.UpdateCreatorWallet(this);
            foreach(var answer in _answers)
            {
                answer.Key.UpdateParticipantWallet(this);
            }
        }

        internal bool IsSuccess() => _status.IsSuccess();

        internal int GetCoins() => _coins;

        internal void AddAnswer(Member member, bool isAccepted, DateTime dateAnswer)
        {
            _answers.Add(member, new Answer(isAccepted, dateAnswer));
            AddDomainEvent(new BetAnswered(_betId.Value, member.Id.Value, isAccepted));
        }

        public static Bet FromState(BetState state)
        {
            return new Bet(state);
        }

        public BetState State
        {
            get => new BetState(_betId.Value,
                        _creator,
                        _endDate.Value,
                        _description,
                        _coins,
                        _creationDate,
                        _answers.Select(x => new AnswerState(x.Key, x.Value.Accepted, x.Value.DateAnswer))
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
            return _answers.FirstOrDefault(x => x.Key.Id.Value == memberId.Value).Value;
        }
    }
}
