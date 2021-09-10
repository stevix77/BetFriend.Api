namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Usecases.InsertBetQuerySide;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using System;
    using System.Threading.Tasks;
    using Xunit;


    public class UpdateFeedHandlerTest
    {
        [Fact]
        public async Task ShouldContainsNewBetInFeed()
        {
            //arrange
            var memberId = Guid.NewGuid();
            var betId = Guid.NewGuid();
            var member = new Member(new(memberId), "toto", 0);
            var bet = Bet.Create(new BetId(betId),
                                new DateTime(2022, 2, 3),
                                "desc1", 10, new(memberId), new DateTime(2021, 3, 2));
            var betRepository = new InMemoryBetRepository(null, bet.State);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var handler = new UpdateFeedMemberCommandHandler(betRepository);
            var notification = new InsertBetQuerySideNotification(betId, memberId);
        }
    }

    internal class UpdateFeedMemberCommandHandler
    {
        public UpdateFeedMemberCommandHandler()
        {
        }
    }
}
