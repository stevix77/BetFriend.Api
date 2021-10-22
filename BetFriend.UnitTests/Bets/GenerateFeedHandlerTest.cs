namespace BetFriend.Bet.UnitTests.Bets
{
    using BetFriend.Bet.Application.Exceptions;
    using BetFriend.Bet.Application.Usecases.GenerateFeed;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;


    public class GenerateFeedHandlerTest
    {
        [Fact]
        public async Task ShouldGenerateFeed()
        {
            var bet = new BetState(Guid.Parse("adc5776b-3596-4dfb-ad0c-691812bfebbd"),
                                        new Member(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")), "toto", 300),
                                        new DateTime(2022, 9, 24),
                                        "description",
                                        30,
                                        new DateTime(2021, 9, 24),
                                        new List<AnswerState>(), null, null);
            var betRepo = new InMemoryBetRepository(null, bet);
            var memberRepo = new InMemoryMemberRepository(new List<Member>() { new Member(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")),
                                                                                          "toto",
                                                                                          300) });
            var command = new GenerateFeedCommand(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")));
            var feedRepo = new InMemoryFeedRepository();
            var handler = new GenerateFeedCommandHandler(feedRepo,
                                                         betRepo,
                                                         memberRepo);
            await handler.Handle(command, default);
            Assert.Single(feedRepo.GetFeeds());
            var feed = feedRepo.GetFeeds().First();
            Assert.Equal(1, feed.Bets.Count);
            Assert.Equal("ddc5776b-3596-4dfb-ad0c-691812bfebbf", feed.Id.ToString());
        }

        [Fact]
        public async Task ShouldNotGenerateFeedIfMemberDoesNotExists()
        {
            var command = new GenerateFeedCommand(new(Guid.Parse("ddc5776b-3596-4dfb-ad0c-691812bfebbf")));
            var feedRepo = new InMemoryFeedRepository();
            var handler = new GenerateFeedCommandHandler(feedRepo,
                                                         new InMemoryBetRepository(),
                                                         new InMemoryMemberRepository());
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));
            Assert.IsType<MemberNotFoundException>(record);
            Assert.Empty(feedRepo.GetFeeds());
        }
    }
}
