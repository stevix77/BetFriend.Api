using BetFriend.Bet.Application.Models;
using BetFriend.Bet.Application.Usecases.GenerateFeed;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Feeds;
using BetFriend.Bet.Domain.Members;
using BetFriend.Bet.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.Bet.UnitTests.Bets
{
    public class GenerateFeedHandlerTest
    {
        [Fact]
        public async Task ShouldGenerateFeed()
        {
            var bets = new List<BetDto>()
            {
                new BetDto(new BetState(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbd"),
                                        new Member(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")), "toto", 300),
                                        new DateTime(2022, 9, 24),
                                        "description",
                                        30,
                                        new DateTime(2021, 9, 24),
                                        null)),
                new BetDto(new BetState(Guid.Parse("adc5776b-3596-4dfb-ad0c-691812bfebbd"),
                                        new Member(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")), "toto", 300),
                                        new DateTime(2022, 9, 24),
                                        "description",
                                        30,
                                        new DateTime(2021, 9, 24),
                                        null)),
            };
            var betQueryRepo = new InMemoryBetQueryRepository(bets);
            var command = new GenerateFeedCommand(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")));
            var feedRepo = new InMemoryFeedRepository();
            var handler = new GenerateFeedCommandHandler(feedRepo, betQueryRepo);
            await handler.Handle(command, default);
            Assert.Single(feedRepo.GetFeeds());
            var feed = feedRepo.GetFeeds().First();
            //Assert.Equal(bets.Take(10).ToList(), feed.Bets.ToList());
        }
    }
}
