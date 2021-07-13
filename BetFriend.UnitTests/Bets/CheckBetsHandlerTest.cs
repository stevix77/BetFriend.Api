namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Usecases.CheckBets;
    using BetFriend.Application.ViewModels;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CheckBetsHandlerTest
    {
        [Fact]
        public async Task HandleShouldReturn0BetsIfMemberHasNeverParticipateToBet()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var query = new CheckBetsQuery(memberId);
            var handler = new CheckBetsQueryHandler(new InMemoryBetQueryRepository(new List<BetViewModel>()));

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
            var query = new CheckBetsQuery(memberId);
            var bets = new List<BetViewModel>
            {
                new BetViewModel
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 1",
                    CreatorId = memberId
                },
                new BetViewModel
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 2",
                    CreatorId = Guid.NewGuid()
                },
                new BetViewModel
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 3",
                    CreatorId = memberId
                }
            };
            IBetQueryRepository betRepository = new InMemoryBetQueryRepository(new(bets));
            var handler = new CheckBetsQueryHandler(betRepository);

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
            var query = new CheckBetsQuery(memberId);
            var bets = new List<BetViewModel>
            {
                new BetViewModel
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 1",
                    CreatorId = Guid.NewGuid(),
                    Participants = new List<MemberViewModel>
                    {
                        new MemberViewModel
                        {
                            Id = memberId,
                            Username = "toto"
                        }
                    }
                },
                new BetViewModel
                {
                    Id = Guid.NewGuid(),
                    Description = "Desc 2",
                    CreatorId = Guid.NewGuid(),
                    Participants = new List<MemberViewModel>
                    {
                        new MemberViewModel
                        {
                            Id = Guid.NewGuid(),
                            Username = "toto"
                        }
                    }
                }
            };
            var betRepository = new InMemoryBetQueryRepository(new(bets));
            var handler = new CheckBetsQueryHandler(betRepository);

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
            var handler = new CheckBetsQueryHandler(betRepository);

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
            var record = Record.Exception(() => new CheckBetsQueryHandler(default));

            //assert
            Assert.IsType<ArgumentNullException>(record);
        }
    }
}
