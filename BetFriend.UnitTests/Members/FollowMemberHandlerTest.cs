namespace BetFriend.UnitTests.Members
{
    using BetFriend.Application.Usecases.FollowMember;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class FollowMemberHandlerTest
    {
        [Fact]
        public async Task ShouldAddMemberToFollowers()
        {
            var memberIdToFollow = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var memberToFollow = new Member(new(memberIdToFollow), "member2", 300);
            var member = new Member(new(memberId), "member1", 300);
            IMemberRepository memberReposiory = new InMemoryMemberRepository(new() { member, memberToFollow });
            var handler = new FollowMemberCommanderHandler(memberReposiory);
            var command = new FollowMemberCommand(memberId, memberIdToFollow);

            await handler.Handle(command, default);

            Assert.Collection(member.Followers, x =>
            {
                Assert.Equal(memberIdToFollow, x.MemberId.Value);
            });
        }
    }
}
