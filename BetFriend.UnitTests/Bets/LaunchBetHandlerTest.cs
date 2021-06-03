namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Usecases.LaunchBet;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Bets.Events;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class LaunchBetHandlerTest
    {
        private const string description = "description";
        private const int tokens = 2;
        private readonly Guid _betId;
        private readonly Guid _creatorId;

        public LaunchBetHandlerTest()
        {
            _betId = Guid.NewGuid();
            _creatorId = Guid.NewGuid();
        }

        [Fact]
        public async Task ShouldCreateBet()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(5);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, tokens, description);
            IBetRepository betRepository = new InMemoryBetRepository();
            var member = new Member(new MemberId(_creatorId), 25);
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() { member });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);
            BetState expectedBet = new(_betId, _creatorId, endDate, description, tokens, DateTime.UtcNow);
            //act
            await handler.Handle(command, default);

            //assert
            Bet actualBet = await betRepository.GetByIdAsync(_betId);
            var domainEvent = actualBet.DomainEvents.FirstOrDefault(x => x.GetType() == typeof(BetCreated));
            Assert.Equal(expectedBet.BetId, actualBet.State.BetId);
            Assert.Equal(expectedBet.Description, actualBet.State.Description);
            Assert.Equal(expectedBet.EndDate, actualBet.State.EndDate);
            Assert.Equal(expectedBet.CreatorId, actualBet.State.CreatorId);
            Assert.Equal(expectedBet.Tokens, actualBet.State.Tokens);
            Assert.True(actualBet.State.CreationDate != DateTime.MinValue);
            Assert.True(expectedBet.EndDate>actualBet.State.CreationDate);
            Assert.NotNull(domainEvent);
        }

        [Fact]
        public async Task ShouldThrowMemberDoesNotEnoughTokensExceptionIfWalletContainsLessThanTokens()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(5);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, tokens, description);
            var member = new Member(new MemberId(_creatorId), 1);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() { member });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberDoesNotEnoughTokensException>(record);
        }

        [Fact]
        public async Task ShouldThrowEndDateNotValidExceptionIfEndDateLessOrEqualDateNow()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(-1);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, tokens, description);
            var member = new Member(new MemberId(_creatorId), 1000);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() { member });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<EndDateNotValidException>(record);
            Assert.Equal("The end date is before the current date", record.Message);
        }

        [Fact]
        public async Task ShouldThrowArgumentExceptionIfBetIdDefault()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(-1);
            var command = new LaunchBetCommand(Guid.Empty, _creatorId, endDate, tokens, description);
            var member = new Member(new MemberId(_creatorId), 1);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() { member });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<ArgumentException>(record);
            Assert.Equal("BetId should be initialized", record.Message);
        }

        [Fact]
        public async Task ShouldThrowMemberUnknownIfCreatorIsNotFound()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(1);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, tokens, description);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() {  });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberUnknownException>(record);
            Assert.Equal($"Creator is unknown", record.Message);
        }

        [Fact]
        public async Task ShouldThrowArgumentNullExceptionIfRequestIsNull()
        {
            //arrange
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository();
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(default, default));

            //assert
            Assert.IsType<ArgumentNullException>(record);
            Assert.Equal("request cannot be null (Parameter 'request')", record.Message);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionIfParametersCtorNull()
        {
            //arrange
            LaunchBetCommandHandler handler;

            //act
            var record1 = Record.Exception(() => handler = new LaunchBetCommandHandler(default, new InMemoryMemberRepository()));
            var record2 = Record.Exception(() => handler = new LaunchBetCommandHandler(new InMemoryBetRepository(), default));

            //assert
            Assert.IsType<ArgumentNullException>(record1);
            Assert.IsType<ArgumentNullException>(record2);
            Assert.Equal("betRepository cannot be null (Parameter 'betRepository')", record1.Message);
            Assert.Equal("memberRepository cannot be null (Parameter 'memberRepository')", record2.Message);
        }
    }
}
