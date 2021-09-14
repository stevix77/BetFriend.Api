namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Application.Usecases.RetrieveBets;
    using BetFriend.Domain.Bets;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class RetrieveBetsHandlerTest
    {
        [Fact]
        public async Task HandleShouldReturn0BetsIfMemberHasNeverParticipateToBet()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var query = new RetrieveBetsQuery(memberId);
            var handler = new RetrieveBetsQueryHandler(new InMemoryBetQueryRepository(new List<BetDto>()));

            //act
            var result = await handler.Handle(query, default);

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task HandleShouldReturnBetsMemberCreated()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var query = new RetrieveBetsQuery(memberId);
            var bets = new List<BetDto>
            {
                new BetDto(new BetState(Guid.NewGuid(),
                                        memberId,
                                        new DateTime(2021, 4, 3), "desc1",
                                        30, new DateTime(2021, 2, 2),
                                        new List<AnswerState>()),
                           new Domain.Members.Member(new(memberId), "member1", 30)),
                new BetDto(new BetState(Guid.NewGuid(),
                                        memberId,
                                        new DateTime(2021, 6, 3), "desc2",
                                        30, new DateTime(2021, 3, 2),
                                        new List<AnswerState>()),
                           new Domain.Members.Member(new(memberId), "member1", 30))
            };
            IBetQueryRepository betRepository = new InMemoryBetQueryRepository(new(bets));
            var handler = new RetrieveBetsQueryHandler(betRepository);

            //act
            var betsResult = await handler.Handle(query, default);

            //assert
            Assert.Equal(2, betsResult.Count);
        }

        [Fact]
        public async Task ShouldReturnBetsMemberHasParticipated()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var query = new RetrieveBetsQuery(memberId);
            var bets = new List<BetDto>
            {
                new BetDto(new BetState(Guid.NewGuid(),
                                        memberId,
                                        new DateTime(2021, 4, 3), "desc1",
                                        30, new DateTime(2021, 2, 2),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(memberId, true, new DateTime(2021, 3, 5))
                                        }),
                           new Domain.Members.Member(new(memberId), "member1", 30))
            };
            var betRepository = new InMemoryBetQueryRepository(new(bets));
            var handler = new RetrieveBetsQueryHandler(betRepository);

            //act
            var betsResult = await handler.Handle(query, default);

            //assert
            Assert.Equal(1, betsResult.Count);
        }

        [Fact]
        public async Task ShouldThrowArgumentNullExceptionIfRequestNull()
        {
            //arrange
            var betRepository = new InMemoryBetQueryRepository(default);
            var handler = new RetrieveBetsQueryHandler(betRepository);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(default, default));

            //assert
            Assert.IsType<ArgumentNullException>(record);
            Assert.Equal("request cannot be null (Parameter 'request')", record.Message);
        }

        [Fact]
        public void CtorShouldThrowArgumentNullExceptionIfInputNull()
        {
            //act
            var record = Record.Exception(() => new RetrieveBetsQueryHandler(default));

            //assert
            Assert.IsType<ArgumentNullException>(record);
        }
    }
}
