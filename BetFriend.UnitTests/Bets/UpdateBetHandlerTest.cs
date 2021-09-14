namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Application.Usecases.UpdateBet;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;


    public class UpdateBetHandlerTest
    {
        /// <summary>
        /// tester les données d'un pari
        /// tester qu'on lève une exception si le pari n'existe pas
        /// </summary>
        [Fact]
        public async Task ShouldThrowBetUnknownExceptionIfBetDoesNotExist()
        {
            var betId = Guid.NewGuid();
            var command = new UpdateBetCommand(betId);
            var handler = new UpdateBetCommandHandler(new InMemoryBetRepository(), null);

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<BetUnknownException>(record);
        }

        [Fact]
        public async Task ShouldUpdateBetIfBetIdKnown()
        {
            var betId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var betState = new BetState(betId,
                                        creatorId,
                                        new DateTime(2021, 8, 4),
                                        "desc2",
                                        20,
                                        new DateTime(2021, 3, 3),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(memberId, true, new DateTime(2021, 4, 2))
                                        });
            IBetRepository betRepository = new InMemoryBetRepository(default, betState);
            var betsDto = new List<BetDto>()
            {
                new BetDto(new(betId, creatorId, new DateTime(2021, 5, 2), "desc", 30, new DateTime(2021, 2, 1), new List<AnswerState>()
                {
                    new AnswerState(memberId, true, new DateTime(2021, 3, 3))
                }),
                            new Domain.Members.Member(new(creatorId), "toto", 100))
            };
            IBetQueryRepository betQueryRepository = new InMemoryBetQueryRepository(betsDto);
            var handler = new UpdateBetCommandHandler(betRepository, betQueryRepository);
            var command = new UpdateBetCommand(betId);

            var result = await handler.Handle(command, default);

            var bet = await betQueryRepository.GetByIdAsync(betId);
            Assert.Equal(Unit.Value, result);
            Assert.Equal(20, bet.Coins);
            Assert.Equal("desc2", bet.Description);
            Assert.Equal(new DateTime(2021, 8, 4), bet.EndDate);
            Assert.Equal(1, bet.Participants.Count);
            Assert.Collection(bet.Participants, x =>
            {
                Assert.Equal(memberId, x.Id);
            });
        }
    }
}
