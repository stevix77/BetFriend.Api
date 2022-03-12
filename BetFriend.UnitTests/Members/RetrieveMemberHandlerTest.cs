namespace BetFriend.Bet.UnitTests.Members
{
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Application.Usecases.RetrieveMember;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;


    public class RetrieveMemberHandlerTest
    {
        [Fact]
        public async Task ShouldNotRetrieveMemberWhenIdUnknown()
        {
            var query = new RetrieveMemberQuery(default);
            var queryHandler = new RetrieveMemberQueryHandler(new InMemoryQueryMemberRepository());
            var member = await queryHandler.Handle(query, default);
            Assert.Null(member);
        }

        [Fact]
        public async Task ShouldRetrieveMemberWhenIdKnown()
        {
            var memberId = Guid.NewGuid();
            var query = new RetrieveMemberQuery(memberId);
            var members = new List<MemberDto>() { new MemberDto() { Id = memberId } };
            var queryHandler = new RetrieveMemberQueryHandler(new InMemoryQueryMemberRepository(members));
            var member = await queryHandler.Handle(query, default);
            Assert.NotNull(member);
        }
    }
}
