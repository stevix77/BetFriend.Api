namespace BetFriend.UnitTests
{
    using BetFriend.Application.Usecases.LaunchBet;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.InMemory.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using static BetFriend.Domain.Bets.Bet;

    public class LaunchBetHandlerTest
    {
        private const string description = "description";
        private readonly BetId _betId;
        private readonly MemberId _creatorId;

        public LaunchBetHandlerTest()
        {
            _betId = new BetId(Guid.NewGuid());
            _creatorId = new MemberId(Guid.NewGuid());
        }

        [Fact]
        public async Task ShouldInsertBetInDbIfValid()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(1);
            var participants = new[] { new MemberId(Guid.NewGuid()) };
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, participants, description);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<MemberId>(participants) { _creatorId });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);
            BetState expectedBet = new(_betId, _creatorId, endDate, description, participants);

            //act
            await handler.Handle(command, default);

            //assert
            Bet actualBet = await betRepository.GetByIdAsync(_betId);
            Assert.Equal(expectedBet, actualBet.State);
        }

        [Fact]
        public async Task ShouldThrowEndDateNotValidExceptionIfEndDateLessOrEqualDateNow()
        {
            //arrange
            var endDate = new DateTime(2020, 05, 05);
            var participants = new[] { new MemberId(Guid.NewGuid()) };
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, participants, description);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<MemberId>(participants) { _creatorId });
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<EndDateNotValidException>(record);
            Assert.Equal("The end date is before the current date", record.Message);
        }

        [Fact]
        public async Task ShouldThrowMemberUnknownIfSomeMemberIsNotFound()
        {
            //arrange
            var endDate = DateTime.UtcNow.AddDays(1);
            var participants = new[] { new MemberId(Guid.NewGuid()) };
            var command = new LaunchBetCommand(_betId, _creatorId, endDate, participants, description);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository();
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberUnknownException>(record);
            Assert.Equal($"Some member are unknown", record.Message);
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
