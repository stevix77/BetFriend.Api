namespace BetFriend.Bet.UnitTests.Bets
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Application.Usecases.UpdateBet;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;


    public class UpdateBetHandlerTest
    {
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
                                        new(new(creatorId), "toto", 300),
                                        new DateTime(2021, 8, 4),
                                        "desc2",
                                        20,
                                        new DateTime(2021, 3, 3),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(new(new(memberId), "name", 300), true, new DateTime(2021, 4, 2))
                                        });
            IBetRepository betRepository = new InMemoryBetRepository(default, betState);
            var betsDto = new List<BetDto>()
            {
                new BetDto(new(betId, 
                            new(new(creatorId), "toto", 300), 
                            new DateTime(2021, 5, 2), 
                            "desc", 
                            30, 
                            new DateTime(2021, 2, 1), 
                            new List<AnswerState>()
                            {
                                new AnswerState(new(new(memberId), "name", 300),
                                                true,
                                                new DateTime(2021, 3, 3))
                            }))
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
