namespace BetFriend.Bet.UnitTests.Bets
{
    using BetFriend.Bet.Application;
    using BetFriend.Bet.Application.Usecases.CloseBet;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Bets.Events;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.DateTimeProvider;
    using BetFriend.Bet.Infrastructure.Gateways;
    using BetFriend.Bet.Infrastructure.Repositories.InMemory;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CloseBetHandlerTest
    {
        [Fact]
        public async Task ShouldCloseBet()
        {
            var betId = Guid.NewGuid();
            var memberId = new MemberId(Guid.Parse("1311e8c9-d993-4346-94c1-1907e3ce65d3"));
            var creator = new Member(memberId, "name", 300);
            BetState betState = GenerateBet(betId, creator);
            var domainEventsListener = new DomainEventsListener();
            var repository = new InMemoryBetRepository(domainEventsListener, betState);
            var command = new CloseBetCommand(betId, memberId.Value, true);
            var dateTimeClosed = new DateTime(2021, 4, 1);
            var dateTimeProvider = new FakeDateTimeProvider(dateTimeClosed);
            var handler = new CloseBetCommandHandler(repository, dateTimeProvider, new InMemoryAuthenticationGateway(memberId.Value));
            var expectedStatus = new Status(true, dateTimeClosed);

            var result = await handler.Handle(command, default);

            var bet = await repository.GetByIdAsync(new(betId));
            Assert.Equal(Unit.Value, result);
            Assert.Equal(expectedStatus, bet.State.Status);
            Assert.Equal(expectedStatus, bet.State.Status);
            var domainEvent = domainEventsListener.GetDomainEvents()
                                                  .SingleOrDefault(x => x.GetType() == typeof(BetClosed)) as BetClosed;
            Assert.NotNull(domainEvent);
            Assert.Equal(betId, domainEvent.BetId);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenMemberIdIsNotCreatorId()
        {
            var betId = Guid.NewGuid();
            var memberId = new MemberId(Guid.NewGuid());
            var betState = GenerateBet(betId, new Member(new MemberId(Guid.NewGuid()), "name", 200));
            var command = new CloseBetCommand(betId, memberId.Value, true);
            var handler = new CloseBetCommandHandler(new InMemoryBetRepository(default, betState),
                                                    new FakeDateTimeProvider(default),
                                                    new InMemoryAuthenticationGateway(memberId.Value));

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<MemberAuthorizationException>(record);
            Assert.Equal($"Member {memberId.Value} is not creator of this bet", record.Message);
        }

        [Fact]
        public async Task ShouldThrowBetUnknownExceptionIfBetNotFound()
        {
            var betId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var command = new CloseBetCommand(betId, memberId, true);
            var handler = new CloseBetCommandHandler(new InMemoryBetRepository(default, default),
                                                    new FakeDateTimeProvider(default),
                                                    new InMemoryAuthenticationGateway(memberId));

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<BetUnknownException>(record);
            Assert.Equal($"This bet with id {betId} is unknown", record.Message);
        }

        [Fact]
        public async Task ShouldThrowNotAuthenticatedException()
        {
            var betId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var command = new CloseBetCommand(betId, memberId, true);
            var handler = new CloseBetCommandHandler(new InMemoryBetRepository(default, default),
                                                    new FakeDateTimeProvider(default),
                                                    new InMemoryAuthenticationGateway(Guid.NewGuid()));

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<NotAuthenticatedException>(record);
        }

        private static BetState GenerateBet(Guid betId, Member creator)
        {
            return new BetState(betId,
                                        creator,
                                        new DateTime(2021, 4, 3),
                                        "description",
                                        45,
                                        new DateTime(2021, 2, 4),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(new(new(Guid.NewGuid()), "name", 300), true, new DateTime(2021, 3, 3))
                                        });
        }
    }
}
