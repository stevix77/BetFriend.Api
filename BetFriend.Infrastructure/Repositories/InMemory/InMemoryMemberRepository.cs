namespace BetFriend.Bet.Infrastructure.Repositories.InMemory
{
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public sealed class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Member> _members;
        private readonly IDomainEventsAccessor _domainEventsListener;

        public InMemoryMemberRepository(List<Member> members = null, IDomainEventsAccessor domainEventsListener = null)
        {
            _members = members ?? new List<Member>();
            _domainEventsListener = domainEventsListener;
        }

        public Task<Member> GetByIdAsync(MemberId memberId)
        {
            return Task.FromResult(_members.Find(x => x.Id.Equals(memberId)));
        }

        public Task<List<Member>> GetByIdsAsync(IEnumerable<MemberId> participantsId)
        {
            return Task.FromResult(_members.Where(x => participantsId.Contains(x.Id))
                                           .ToList());
        }

        public Task SaveAsync(Member member)
        {
            _members.Add(member);
            return Task.CompletedTask;
        }

        public Task SaveAsync(IReadOnlyCollection<Member> members)
        {
            _domainEventsListener?.AddDomainEvents(members.SelectMany(x => x.DomainEvents).ToList());
            return Task.CompletedTask;
        }

        public IEnumerable GetMembers()
        {
            return _members;
        }
    }
}
