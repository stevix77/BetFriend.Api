namespace BetFriend.Bet.UnitTests.Members
{
    using BetFriend.Bet.Application.Usecases.CreateMember;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using System;
    using System.Threading.Tasks;
    using Xunit;


    public class CreateMemberHandlerTest
    {
        [Fact]
        public async Task ShouldCreateMember()
        {
            var memberId = Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1");
            var command = new CreateMemberCommand(memberId,
                                                  "toto");
            var memberRepository = new InMemoryMemberRepository();
            var handler = new CreateMemberCommandHandler(memberRepository);
            await handler.Handle(command, default);
            var member = await memberRepository.GetByIdAsync(new(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1")));
            Assert.Equal(memberId, member.Id.Value);
            Assert.Equal("toto", member.Name);
            Assert.Equal(1000, member.Wallet);
        }

        [Fact]
        public async Task ShouldNotCreateMemberIfMemberAlreadyCreated()
        {
            var memberId = Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1");
            var command = new CreateMemberCommand(memberId,
                                                  "toto");
            var member = new Member(new(memberId), "toto", 10);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var handler = new CreateMemberCommandHandler(memberRepository);
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));
            Assert.IsType<MemberAlreadyExistsException>(record);
            Assert.Single(memberRepository.GetMembers());
        }
    }
}
