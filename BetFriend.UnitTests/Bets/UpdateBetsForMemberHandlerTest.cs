using BetFriend.Application.Usecases.InsertBetQuerySide;
using BetFriend.Application.Models;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Members;
using BetFriend.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UnitTests.Bets
{
    public class UpdateBetsForMemberHandlerTest
    {
        [Fact]
        public async Task ShouldAddBetIfBetExists()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var betId = Guid.NewGuid();
            var member = new Member(new(memberId), "toto", 0);
            var bet = Bet.Create(new BetId(betId),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, new(memberId), new DateTime(2021, 3, 2));
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var queryBetRepository = new InMemoryBetQueryRepository(new List<BetDto>());
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var command = new InsertBetQuerySideCommand(betId, memberId);
            var handler = new InsertBetQuerySideCommandHandler(betRepository, queryBetRepository, memberRepository);

            //act
            await handler.Handle(command, default);

            //assert
            var bets = await queryBetRepository.GetBetsForMemberAsync(memberId);
            Assert.Collection(bets, (x) =>
            {
                Assert.Equal(memberId, x.CreatorId);
                Assert.Equal(member.MemberName, x.CreatorUsername);
                Assert.Equal(bet.State.Coins, x.Coins);
                Assert.Equal(bet.State.Description, x.Description);
                Assert.Equal(bet.State.EndDate, x.EndDate);
                Assert.Equal(bet.State.BetId, x.Id);
            });
        }

        [Fact]
        public async Task ShouldThrowBetUnknownExceptionIfBetIdUnknown()
        {
            //arrange
            var betId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var bet = Bet.Create(new BetId(Guid.NewGuid()),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, new(memberId), new DateTime(2021, 3, 2));
            var member = new Member(new(memberId), "toto", 0);
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var queryBetRepository = new InMemoryBetQueryRepository(new List<BetDto>());
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var command = new InsertBetQuerySideCommand(betId, memberId);
            var handler = new InsertBetQuerySideCommandHandler(betRepository, queryBetRepository, memberRepository);

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
            var command = new InsertBetQuerySideCommand(Guid.NewGuid(), Guid.NewGuid());
            var handler = new InsertBetQuerySideCommandHandler(betRepository, queryBetRepository, memberRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberUnknownException>(record);
        }

        [Fact]
        public async Task ShouldThrowArgumentNullExceptionIfRequestNull()
        {
            //arrange
            var handler = new InsertBetQuerySideCommandHandler(new InMemoryBetRepository(), new InMemoryBetQueryRepository(), new InMemoryMemberRepository());

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(default, default));

            //assert
            Assert.IsType<ArgumentNullException>(record);
        }
    }
}
