namespace BetFriend.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class LaunchBetHandlerTest
    {
        [Fact]
        public async Task ShouldInsertBetInDbIfValid()
        {
            //arrange
            var bettorId = new BettorId(Guid.NewGuid());
            var endDate = new DateTime(2021, 05, 05);
            var participants = new[] { "0101020203" };
            var betId = new BetId(Guid.NewGuid());
            var command = new LaunchBetCommand(betId, bettorId, endDate, participants);
            IUserRepository userRepository = null;
            IBetRepository betRepository = null;
            var handler = new LaunchBetCommandandler(userRepository, betRepository);
            Bet expectedBet = null;

            //act
            await handler.Handle(command, default);

            //assert
            Bet actualBet = betRepository.GetByIdAsync(betId);
            Assert.Equal(expectedBet, actualBet);

        }
    }

    internal class Bet
    {
    }

    internal interface IBetRepository
    {
        Bet GetByIdAsync(BetId betId);
    }

    internal interface IUserRepository
    {
    }

    internal struct BetId
    {
        private Guid guid;

        public BetId(Guid guid)
        {
            this.guid = guid;
        }
    }

    internal struct BettorId
    {
        private Guid guid;

        public BettorId(Guid guid)
        {
            this.guid = guid;
        }
    }

    internal class LaunchBetCommand
    {
        public LaunchBetCommand()
        {
        }

        public LaunchBetCommand(BetId betId, BettorId bettorId, DateTime endDate, string[] participants)
        {
            BetId = betId;
            BettorId = bettorId;
            EndDate = endDate;
            Participants = participants;
        }

        public BetId BetId { get; }
        public BettorId BettorId { get; }
        public DateTime EndDate { get; }
        public string[] Participants { get; }
    }

    internal class LaunchBetCommandandler
    {
        private IUserRepository userRepository;
        private IBetRepository betRepository;

        public LaunchBetCommandandler(IUserRepository userRepository, IBetRepository betRepository)
        {
            this.userRepository = userRepository;
            this.betRepository = betRepository;
        }

        internal Task Handle(LaunchBetCommand command, object p)
        {
            throw new NotImplementedException();
        }
    }
}
