namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Bet.Application;
    using BetFriend.Bet.Application.Usecases.LaunchBet;
    using BetFriend.Bet.Domain;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Bets.Events;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.DateTimeProvider;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class LaunchBetHandlerTest
    {
        private const string description = "description";
        private const int coins = 2;
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
            IDateTimeProvider dtNow = new FakeDateTimeProvider(new DateTime(2021, 5, 6, 0, 0, 0));
            var endDate = new DateTime(2021, 5, 7, 0, 0, 0);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, coins, description, dtNow);
            var domainEventsListener = new DomainEventsListener();
            var betRepository = new InMemoryBetRepository(domainEventsListener);
            var member = new Member(new MemberId(_creatorId), "name", 25);
            var memberRepository = new InMemoryMemberRepository(new List<Member>() { member });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);
            BetState expectedBet = new(_betId, member, endDate, description, coins, dtNow.Now, new List<AnswerState>());

            //act
            await handler.Handle(command, default);

            //assert
            Bet actualBet = await betRepository.GetByIdAsync(new(_betId));
            IDomainEvent domainEvent = domainEventsListener.GetDomainEvents()
                                                           .SingleOrDefault(x => x.GetType() == typeof(BetCreated));
            Assert.Equal(expectedBet.BetId, actualBet.State.BetId);
            Assert.Equal(expectedBet.Description, actualBet.State.Description);
            Assert.Equal(expectedBet.EndDate, actualBet.State.EndDate);
            Assert.Equal(expectedBet.Creator.Id, actualBet.State.Creator.Id);
            Assert.Equal(expectedBet.Creator.Name, actualBet.State.Creator.Name);
            Assert.Equal(expectedBet.Creator.Wallet, actualBet.State.Creator.Wallet);
            Assert.Equal(expectedBet.Coins, actualBet.State.Coins);
            Assert.True(actualBet.State.CreationDate != DateTime.MinValue);
            Assert.True(expectedBet.EndDate > actualBet.State.CreationDate);
            Assert.True(actualBet.GetEndDateToAnswer().CompareTo(new DateTime(2021, 5, 6, 6, 0, 0)) == 0);
            Assert.NotNull(domainEvent);
            Assert.Equal(_betId, ((BetCreated)domainEvent).BetId.Value);
            Assert.Equal(_creatorId, ((BetCreated)domainEvent).CreatorId.Value);
        }

        [Fact]
        public async Task ShouldThrowMemberHasNotEnoughCoinsExceptionIfWalletContainsLessThanTokens()
        {
            //arrange
            var dtNow = new FakeDateTimeProvider(new DateTime(2021, 5, 6));
            var endDate = DateTime.UtcNow.AddDays(5);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, coins, description, dtNow);
            var member = new Member(new MemberId(_creatorId), "name", 1);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() { member });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberHasNotEnoughCoinsException>(record);
        }

        [Fact]
        public async Task ShouldThrowEndDateNotValidExceptionIfEndDateLessOrEqualDateNow()
        {
            //arrange
            var dtNow = new FakeDateTimeProvider(new DateTime(2021, 5, 6));
            var endDate = dtNow.Now.AddDays(-1);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, coins, description, dtNow);
            var member = new Member(new MemberId(_creatorId), "name", 1000);
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
            var dtNow = new FakeDateTimeProvider(new DateTime(2021, 5, 6));
            var endDate = DateTime.UtcNow.AddDays(-1);
            var command = new LaunchBetCommand(Guid.Empty, _creatorId, endDate, coins, description, dtNow);
            var member = new Member(new MemberId(_creatorId), "name", 1);
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
            var dtNow = new FakeDateTimeProvider(new DateTime(2021, 5, 6));
            var endDate = DateTime.UtcNow.AddDays(1);
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, coins, description, dtNow);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<Member>() { });
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
