namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Usecases.InsertBetQuerySide;
    using BetFriend.Application.Usecases.UpdateFeedMember;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Feeds;
    using BetFriend.Domain.Followers;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.Repositories.InMemory;
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
            var followerId = Guid.NewGuid();
            var followerId2 = Guid.NewGuid();
            member.AddFollower(new Follower(new(followerId)));
            member.AddFollower(new Follower(new(followerId2)));
            var bet = Bet.Create(new BetId(betId),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, new(memberId), new DateTime(2021, 3, 2));
            Feed feed = Feed.Create(followerId);
            Feed feed2 = Feed.Create(followerId2);
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var feedRepository = new InMemoryFeedRepository(new List<Feed>() { feed, feed2 });
            var handler = new UpdateFeedMemberNotificationHandler(betRepository, memberRepository, feedRepository);
            var notification = new InsertBetQuerySideNotification(betId, memberId);

            //act
            await handler.Handle(notification, default);

            //assert
            await AssertFeedFollower(betId, followerId, bet, feedRepository);
            await AssertFeedFollower(betId, followerId2, bet, feedRepository);
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

        private static async Task AssertFeedFollower(Guid betId, Guid followerId, Bet bet, InMemoryFeedRepository feedRepository)
        {
            Feed feedToAssert = await feedRepository.GetByIdAsync(followerId);
            Assert.NotNull(feedToAssert);
            Assert.Equal(followerId, feedToAssert.Id);
            Assert.Single(feedToAssert.Bets);
            Assert.Collection(feedToAssert.Bets, x =>
            {
                Assert.Equal(betId, x.BetId);
                Assert.Equal(10, x.Coins);
                Assert.Equal("desc1", x.Description);
                Assert.Equal(bet.State.EndDate, x.EndDate);
                Assert.Equal(bet.State.CreatorId, x.CreatorId);
                Assert.Equal(bet.State.CreationDate, x.CreationDate);
            });
        }
    }
}
