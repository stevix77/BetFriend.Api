using BetFriend.Application;
using BetFriend.Application.Usecases.AnswerBet;
using BetFriend.Domain;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Bets.Events;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Members;
using BetFriend.Infrastructure.DateTimeProvider;
using BetFriend.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UnitTests.Bets
{
    public class AnswerBetHandlerTest
    {
        [Fact]
        public async Task HandlerShouldThrowMemberUnknownExceptionIfMemberUnknown()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var member = new Member(memberId, 200);
            var memberRepository = new InMemoryMemberRepository(new());
            var command = new AnswerBetCommand(memberId.Value, Guid.Empty, true, new FakeDateTimeProvider(DateTime.Now));
            var handler = new AnswerBetCommandHandler(memberRepository, new InMemoryBetRepository(), new DomainEventsListener());

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command));

            //assert 
            Assert.IsType<MemberUnknownException>(record);
        }

        [Fact]
        public async Task HandlerShouldThrowBetUnknownExceptionIfBetIdUnknown()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var betId = new BetId(Guid.NewGuid());
            var member = new Member(memberId, 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betRepository = new InMemoryBetRepository(
                    new BetState(betId.Value, 
                                memberId.Value, 
                                DateTime.UtcNow, 
                                "descr", 
                                30, 
                                DateTime.UtcNow.AddSeconds(-1000), 
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()))
                    );
            var command = new AnswerBetCommand(memberId.Value, Guid.Empty, true, new FakeDateTimeProvider(DateTime.Now));
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, new DomainEventsListener());

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command));

            //assert 
            Assert.IsType<BetUnknownException>(record);
            Assert.Equal($"Bet {Guid.Empty} is unknown", record.Message);
        }

        [Fact]
        public async Task HandleShouldThrowArgumentNullExceptionIfRequestNull()
        {
            //arrange
            var memberRepository = new InMemoryMemberRepository();
            var betRepository = new InMemoryBetRepository();
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, new DomainEventsListener());

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(null));

            //assert
            Assert.IsType<ArgumentNullException>(record);
            Assert.Equal("request cannot be null (Parameter 'request')", record.Message);
        }

        [Fact]
        public async Task HandleShouldAddAnswerWhenRequestValide()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var betId = new BetId(Guid.NewGuid());
            var dateTimeAnswerBet = new DateTime(2021, 3, 18, 20, 30, 2);
            var member = new Member(memberId, 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betState = new BetState(betId.Value, memberId.Value, new DateTime(2022, 3, 3), "descr", 30, new DateTime(2021, 2, 3),
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()));
            var betRepository = new InMemoryBetRepository(betState);
            var domainEventsListener = new DomainEventsListener();
            var command = new AnswerBetCommand(memberId.Value, betId.Value, true, new FakeDateTimeProvider(dateTimeAnswerBet));
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, domainEventsListener);

            //act
            await handler.Handle(command);

            //assert
            var actualBet = await betRepository.GetByIdAsync(betId.Value);
            var answer = actualBet.GetAnswerForMember(memberId);
            IDomainEvent domainEvent = domainEventsListener.GetDomainEvents()
                                                           .SingleOrDefault(x => x.GetType() == typeof(BetAnswered));
            Assert.Equal(command.IsAccepted, answer.Accepted);
            Assert.Equal(command.DateAnswer.GetDateTime(), answer.DateAnswer);
            Assert.NotNull(domainEvent);
            Assert.Equal(betId.Value, (domainEvent as BetAnswered).BetId);
            Assert.Equal(memberId.Value, (domainEvent as BetAnswered).MemberId);
            Assert.Equal(command.IsAccepted, (domainEvent as BetAnswered).IsAccepted);
        }

        [Fact]
        public async Task HandleShouldThrowAnswerTooLateExceptionIfDateToAnswerExpired()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var betId = new BetId(Guid.NewGuid());
            var dateTimeAnswerBet = new DateTime(2021, 3, 1);
            var member = new Member(memberId, 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betState = new BetState(betId.Value, memberId.Value, new DateTime(2021, 3, 3), "descr", 30, new DateTime(2021, 2, 3),
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()));
            var betRepository = new InMemoryBetRepository(betState);
            var domainEventsListener = new DomainEventsListener();
            var command = new AnswerBetCommand(memberId.Value, betId.Value, true, new FakeDateTimeProvider(dateTimeAnswerBet));
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, domainEventsListener);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command));

            //assert
            Assert.IsType<AnswerTooLateException>(record);
            Assert.Equal($"The date limit to answer was at : {Bet.FromState(betState).GetEndDateToAnswer().ToLongDateString()}", 
                        record.Message);
        }
    }
}
