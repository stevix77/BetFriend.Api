using BetFriend.Application.Models;
using BetFriend.Application.Usecases.RetrieveBet;
using BetFriend.Domain.Exceptions;
using BetFriend.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UnitTests.Bets
{
    public class RetrieveBetHandlerTest
    {
        [Fact]
        public async Task ShouldThrowExceptionIfBetIdUnknown()
        {
            var betId = Guid.NewGuid();
            var handler = new RetrieveBetQueryHandler(new InMemoryBetQueryRepository());

            var record = await Record.ExceptionAsync(() => handler.Handle(new RetrieveBetQuery(betId), default));

            Assert.IsType<BetUnknownException>(record);
            Assert.Equal($"This bet with id: {betId} does not exist", record.Message);
        }

        [Fact]
        public async Task ShouldReturnBet()
        {
            var betId = Guid.NewGuid();
            var bets = new List<BetDto>() { new BetDto(new(betId,
                                                            new(new(Guid.NewGuid()), "username", 300),
                                                            new DateTime(2021, 10, 10),
                                                            "description",
                                                            20,
                                                            new DateTime(2021, 3, 3),
                                                            null))
                                           };
            var betQueryRepository = new InMemoryBetQueryRepository(bets);
            var handler = new RetrieveBetQueryHandler(betQueryRepository);
            var query = new RetrieveBetQuery(betId);

            var betDto = await handler.Handle(query, default);

            Assert.Equal("description", betDto.Description);
            Assert.Equal(20, betDto.Coins);
            Assert.Equal(new DateTime(2021, 10, 10), betDto.EndDate);
            Assert.Equal(betId, betDto.Id);
        }
    }
}
