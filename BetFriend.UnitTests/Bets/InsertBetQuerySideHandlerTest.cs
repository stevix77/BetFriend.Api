using BetFriend.Bet.Application.Usecases.InsertBetQuerySide;
using BetFriend.Bet.Application.Models;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Exceptions;
using BetFriend.Bet.Domain.Members;
using BetFriend.Bet.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UnitTests.Bets
{
    public class InsertBetQuerySideHandlerTest
    {
        [Fact]
        public async Task ShouldAddBetIfBetExists()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var betId = Guid.NewGuid();
            var member = new Member(new(memberId), "toto", 0);
            var bet = Bet.Domain.Bets.Bet.Create(new BetId(betId),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, member, new DateTime(2021, 3, 2));
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var queryBetRepository = new InMemoryBetQueryRepository(new List<BetDto>());
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var command = new InsertBetQuerySideNotification(betId, memberId);
            var handler = new InsertBetQuerySideNotificationHandler(betRepository, queryBetRepository, memberRepository);

            //act
            await handler.Handle(command, default);

            //assert
            BetDto betInserted = await queryBetRepository.GetByIdAsync(betId);
            Assert.NotNull(betInserted);
            Assert.Equal(bet.State.Coins, betInserted.Coins);
            Assert.Equal(bet.State.Description, betInserted.Description);
            Assert.Equal(bet.State.EndDate, betInserted.EndDate);
            Assert.Equal(bet.State.BetId, betInserted.Id);
            Assert.Equal(bet.State.Creator.Id.Value, betInserted.Creator.Id);
        }

        [Fact]
        public async Task ShouldThrowBetUnknownExceptionIfBetIdUnknown()
        {
            //arrange
            var betId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var member = new Member(new(memberId), "toto", 0);
            var bet = Bet.Domain.Bets.Bet.Create(new BetId(Guid.NewGuid()),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, member, new DateTime(2021, 3, 2));
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var queryBetRepository = new InMemoryBetQueryRepository(new List<BetDto>());
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var command = new InsertBetQuerySideNotification(betId, memberId);
            var handler = new InsertBetQuerySideNotificationHandler(betRepository, queryBetRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<BetUnknownException>(record);
        }

        [Fact]
        public async Task ShouldThrowMemberUnknownExceptionIfMemberIdUnknown()
        {
            //arrange
            var betRepository = new InMemoryBetRepository(null, null);
            var queryBetRepository = new InMemoryBetQueryRepository(new List<BetDto>());
            var memberRepository = new InMemoryMemberRepository();
            var command = new InsertBetQuerySideNotification(Guid.NewGuid(), Guid.NewGuid());
            var handler = new InsertBetQuerySideNotificationHandler(betRepository, queryBetRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberUnknownException>(record);
        }

        [Fact]
        public async Task ShouldThrowArgumentNullExceptionIfRequestNull()
        {
            //arrange
            var handler = new InsertBetQuerySideNotificationHandler(new InMemoryBetRepository(), new InMemoryBetQueryRepository(), new InMemoryMemberRepository());

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(default, default));

            //assert
            Assert.IsType<ArgumentNullException>(record);
        }
    }
}
