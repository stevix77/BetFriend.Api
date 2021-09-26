namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Domain.Members;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Member> _members;
        private readonly IDomainEventsListener _domainEventsListener;

        public InMemoryMemberRepository(List<Member> members = null, IDomainEventsListener domainEventsListener = null)
        {
            _members = members ?? (_members = new List<Member>());
            _domainEventsListener = domainEventsListener;
        }

        public Task<Member> GetByIdAsync(MemberId memberId)
        {
            return Task.FromResult(_members.Find(x => x.Id.Equals(memberId)));
        }

        public Task<List<Member>> GetByIdsAsync(IEnumerable<MemberId> memberIds)
        {
            return Task.FromResult(_members.Where(x => memberIds.Contains(x.Id))
                                           .ToList());
        }

        public Task SaveAsync(IReadOnlyCollection<Member> members)
        {
            foreach(var member in members)
            {
                if (!_members.Any(x => x.Id.Equals(member.Id)))
                    _members.Add(member);
            }
            _domainEventsListener?.AddDomainEvents(members.SelectMany(x => x.DomainEvents).ToList());
            return Task.CompletedTask;
        }

        public IEnumerable GetMembers()
        {
            return _members;
        }

        public Task SaveAsync(Member member)
        {
            if (_members.Any(x => x.Id.Equals(member.Id)))
                return Task.CompletedTask;
            _members.Add(member);
            return Task.CompletedTask;
        }
    }
}
