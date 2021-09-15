﻿namespace BetFriend.UnitTests.Bets
{
    using BetFriend.Application;
    using BetFriend.Application.Usecases.CloseBet;
    using BetFriend.Domain;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.DateTimeProvider;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// terminer le pari
    /// verifier que le user est le creator
    /// donner un resutat (bool) à une date avec un byte[]
    /// mettre à jour le pari
    /// </summary>
    public class CloseBetHandlerTest
    {
        [Fact]
        public async Task ShouldCloseBet()
        {
            var betId = Guid.NewGuid();
            var memberId = new MemberId(Guid.Parse("1311e8c9-d993-4346-94c1-1907e3ce65d3"));
            var creator = new Member(memberId, "name", 300);
            var betState = new BetState(betId,
                                        creator,
                                        new DateTime(2021, 4, 3),
                                        "description",
                                        45,
                                        new DateTime(2021, 2, 4),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(Guid.NewGuid(), true, new DateTime(2021, 3, 3))
                                        });
            var domainEventsListener = new DomainEventsListener();
            var repository = new InMemoryBetRepository(domainEventsListener, betState);
            var command = new CloseBetCommand(betId, memberId.Value, true);
            var dateTimeClosed = new DateTime(2021, 4, 1);
            var dateTimeProvider = new FakeDateTimeProvider(dateTimeClosed);
            var handler = new CloseBetCommandHandler(repository, dateTimeProvider);
            var expectedStatus = new Status(true, dateTimeClosed);

            var result = await handler.Handle(command, default);

            var bet = await repository.GetByIdAsync(betId);
            Assert.Equal(Unit.Value, result);
            Assert.Equal(expectedStatus, bet.State.Status);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenMemberIdIsNotCreatorId()
        {
            var betId = Guid.NewGuid();
            var memberId = new MemberId(Guid.NewGuid());
            var betState = new BetState(betId,
                                        new(new(Guid.NewGuid()), "username", 300),
                                        new DateTime(2021, 4, 3),
                                        "description",
                                        45,
                                        new DateTime(2021, 2, 4),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(Guid.NewGuid(), true, new DateTime(2021, 3, 3))
                                        });
            var command = new CloseBetCommand(betId, memberId.Value, true);
            var handler = new CloseBetCommandHandler(new InMemoryBetRepository(default, betState),
                                                    new FakeDateTimeProvider(default));

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<MemberAuthorizationException>(record);
            Assert.Equal($"Member {memberId.Value} is not creator of this bet", record.Message);
        }

        //test si bet unknown
    }
}