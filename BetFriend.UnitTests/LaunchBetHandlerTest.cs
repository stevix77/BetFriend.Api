namespace BetFriend.UnitTests
{
    using BetFriend.Application.Usecases.LaunchBet;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class LaunchBetHandlerTest
    {
        [Fact]
        public async Task ShouldInsertBetInDbIfValid()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var endDate = DateTime.UtcNow.AddDays(1);
            var participants = new[] { new MemberId(Guid.NewGuid()) };
            var betId = new BetId(Guid.NewGuid());
            var command = new LaunchBetCommand(betId, memberId, endDate, participants);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository(new List<MemberId>(participants));
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);
            Bet expectedBet = Bet.Create(betId, memberId, endDate, participants);

            //act
            await handler.Handle(command, default);

            //assert
            Bet actualBet = await betRepository.GetByIdAsync(betId);
            Assert.Equal(expectedBet.GetId(), actualBet.GetId());
        }

        [Fact]
        public async Task ShouldThrowEndDateNotValidExceptionIfEndDateLessOrEqualDateNow()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var endDate = new DateTime(2020, 05, 05);
            var participants = new[] { new MemberId(Guid.NewGuid()) };
            var betId = new BetId(Guid.NewGuid());
            var command = new LaunchBetCommand(betId, memberId, endDate, participants);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository();
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
            var memberId = new MemberId(Guid.NewGuid());
            var endDate = DateTime.UtcNow.AddDays(1);
            var participants = new[] { new MemberId(Guid.NewGuid()) };
            var betId = new BetId(Guid.NewGuid());
            var command = new LaunchBetCommand(betId, memberId, endDate, participants);
            IBetRepository betRepository = new InMemoryBetRepository();
            IMemberRepository memberRepository = new InMemoryMemberRepository();
            var handler = new LaunchBetCommandHandler(betRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberUnknownException>(record);
            Assert.Equal($"Some member are unknown", record.Message);
        }
    }



    internal class InMemoryBetRepository : IBetRepository
    {
        private Bet _bet;
        public Task AddAsync(Bet bet)
        {
            _bet = bet;
            return Task.CompletedTask;
        }

        public Task<Bet> GetByIdAsync(BetId betId)
        {
            if (betId.Equals(_bet.GetId()))
                return Task.FromResult(_bet);
            return Task.FromResult<Bet>(null);

        }
    }


}
