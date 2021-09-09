namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Application.Usecases.RetrieveBets;
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
                new BetDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 1",
                    CreatorId = memberId
                },
                new BetDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 2",
                    CreatorId = Guid.NewGuid()
                },
                new BetDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 3",
                    CreatorId = memberId
                }
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
                new BetDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 1",
                    CreatorId = Guid.NewGuid(),
                    Participants = new List<MemberDto>
                    {
                        new MemberDto
                        {
                            Id = memberId,
                            Username = "toto"
                        }
                    }
                },
                new BetDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 2",
                    CreatorId = Guid.NewGuid(),
                    Participants = new List<MemberDto>
                    {
                        new MemberDto
                        {
                            Id = Guid.NewGuid(),
                            Username = "toto"
                        }
                    }
                }
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
