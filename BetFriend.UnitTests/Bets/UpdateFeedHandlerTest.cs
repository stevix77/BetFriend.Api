namespace BetFriend.Bet.UnitTests.Bets
{
    using BetFriend.Bet.Application.Usecases.InsertBetQuerySide;
    using BetFriend.Bet.Application.Usecases.UpdateFeedMember;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Feeds;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Domain.Subscriptions;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;


    public class UpdateFeedHandlerTest
    {
        [Fact]
        public async Task ShouldContainsNewBetInFollowersFeed()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var betId = Guid.NewGuid();
            var member = new Member(new(memberId), "toto", 0);
            var subscriptionId = Guid.NewGuid();
            var subscriptionId2 = Guid.NewGuid();
            member.Subscribe(new Subscription(new(subscriptionId)));
            member.Subscribe(new Subscription(new(subscriptionId2)));
            var bet = Bet.Create(new BetId(betId),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, new(new(memberId), "toto", 300), new DateTime(2021, 3, 2));
            Feed feed = Feed.Create(subscriptionId);
            Feed feed2 = Feed.Create(subscriptionId2);
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var feedRepository = new InMemoryFeedRepository(new List<Feed>() { feed, feed2 });
            var handler = new UpdateFeedMemberNotificationHandler(betRepository, memberRepository, feedRepository);
            var notification = new InsertBetQuerySideNotification(betId, memberId);

            //act
            await handler.Handle(notification, default);

            //assert
            await AssertFeedSubscription(betId, subscriptionId, bet, feedRepository);
            await AssertFeedSubscription(betId, subscriptionId2, bet, feedRepository);
        }

        [Fact]
        public async Task ShouldThrowBetNotFoundExceptionIfBetIdUnknown()
        {
            //arrange
            var notification = new InsertBetQuerySideNotification(Guid.NewGuid(), Guid.NewGuid());
            var handler = new UpdateFeedMemberNotificationHandler(new InMemoryBetRepository(),
                                                                   new InMemoryMemberRepository(),
                                                                   new InMemoryFeedRepository());

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(notification, default));

            //assert
            Assert.IsType<BetUnknownException>(record);
        }

        private static async Task AssertFeedSubscription(Guid betId, Guid subscriptionId, Bet bet, InMemoryFeedRepository feedRepository)
        {
            Feed feedToAssert = await feedRepository.GetByIdAsync(subscriptionId);
            Assert.NotNull(feedToAssert);
            Assert.Equal(subscriptionId, feedToAssert.Id);
            Assert.Single(feedToAssert.Bets);
            Assert.Collection(feedToAssert.Bets, x =>
            {
                Assert.Equal(betId, x.BetId);
                Assert.Equal(10, x.Coins);
                Assert.Equal("desc1", x.Description);
                Assert.Equal(bet.State.EndDate, x.EndDate);
                Assert.Equal(bet.State.Creator.Id, x.Creator.Id);
                Assert.Equal(bet.State.CreationDate, x.CreationDate);
            });
        }
    }
}
