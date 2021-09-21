using BetFriend.Bet.Application;
using BetFriend.Bet.Application.Usecases.AnswerBet;
using BetFriend.Bet.Domain;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Bets.Events;
using BetFriend.Bet.Domain.Exceptions;
using BetFriend.Bet.Domain.Members;
using BetFriend.Bet.Infrastructure.DateTimeProvider;
using BetFriend.Bet.Infrastructure.Gateways;
using BetFriend.Bet.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.Bet.UnitTests.Bets
{
    public class AnswerBetHandlerTest
    {
        [Fact]
        public async Task HandlerShouldThrowMemberUnknownExceptionIfMemberUnknown()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var member = new Member(memberId, "name", 200);
            var memberRepository = new InMemoryMemberRepository(new());
            var command = new AnswerBetCommand(memberId.Value, Guid.Empty, true, new FakeDateTimeProvider(DateTime.Now));
            var authentificationGateway = new InMemoryAuthenticationGateway(memberId.Value);
            var handler = new AnswerBetCommandHandler(memberRepository, new InMemoryBetRepository(), authentificationGateway);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert 
            Assert.IsType<MemberUnknownException>(record);
        }

        [Fact]
        public async Task HandlerShouldThrowBetUnknownExceptionIfBetIdUnknown()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var betId = new BetId(Guid.NewGuid());
            var member = new Member(memberId, "name", 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betRepository = new InMemoryBetRepository(null,
                    new BetState(Guid.NewGuid(),
                                member,
                                DateTime.UtcNow,
                                "descr",
                                30,
                                DateTime.UtcNow.AddSeconds(-1000),
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()))
                    );
            var command = new AnswerBetCommand(memberId.Value, betId.Value, true, new FakeDateTimeProvider(DateTime.Now));
            var authentificationGateway = new InMemoryAuthenticationGateway(memberId.Value);
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, authentificationGateway);

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert 
            Assert.IsType<BetUnknownException>(record);
            Assert.Equal($"Bet {betId.Value} is unknown", record.Message);
        }

        [Fact]
        public async Task HandleShouldThrowArgumentNullExceptionIfRequestNull()
        {
            //arrange
            var memberRepository = new InMemoryMemberRepository();
            var betRepository = new InMemoryBetRepository();
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, new InMemoryAuthenticationGateway(default));

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(null, default));

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
            var member = new Member(memberId, "name", 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betState = new BetState(betId.Value, member, new DateTime(2022, 3, 3), "descr", 30, new DateTime(2021, 2, 3),
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()));
            var domainEventsListener = new DomainEventsListener();
            var betRepository = new InMemoryBetRepository(domainEventsListener, betState);
            var command = new AnswerBetCommand(memberId.Value, betId.Value, true, new FakeDateTimeProvider(dateTimeAnswerBet));
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, new InMemoryAuthenticationGateway(memberId.Value));

            //act
            await handler.Handle(command, default);

            //assert
            var actualBet = await betRepository.GetByIdAsync(betId);
            var answer = actualBet.GetAnswerForMember(memberId);
            IDomainEvent domainEvent = domainEventsListener.GetDomainEvents()
                                                           .SingleOrDefault(x => x.GetType() == typeof(BetAnswered));
            Assert.Equal(command.IsAccepted, answer.Accepted);
            Assert.Equal(command.DateTimeProvider.Now, answer.DateAnswer);
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
            var member = new Member(memberId, "name", 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betState = new BetState(betId.Value, member, new DateTime(2021, 3, 3), "descr", 30, new DateTime(2021, 2, 3),
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()));
            var betRepository = new InMemoryBetRepository(null, betState);
            var command = new AnswerBetCommand(memberId.Value, betId.Value, true, new FakeDateTimeProvider(dateTimeAnswerBet));
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, new InMemoryAuthenticationGateway(memberId.Value));

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<AnswerTooLateException>(record);
            Assert.Equal($"The date limit to answer was at : {Bet.Domain.Bets.Bet.FromState(betState).GetEndDateToAnswer().ToLongDateString()}",
                        record.Message);
        }

        [Fact]
        public async Task HandleShouldThrowMemberNotEnoughCoinsExceptionIfWalletContainsLessCoinsThatBet()
        {
            //arrange
            var memberId = new MemberId(Guid.NewGuid());
            var betId = new BetId(Guid.NewGuid());
            var dateTimeAnswerBet = new DateTime(2021, 3, 1);
            var member = new Member(memberId, "name", 200);
            var memberRepository = new InMemoryMemberRepository(new() { member });
            var betState = new BetState(betId.Value, member, new DateTime(2022, 3, 3), "descr", 300, new DateTime(2021, 2, 3),
                                new ReadOnlyCollection<AnswerState>(new List<AnswerState>()));
            var betRepository = new InMemoryBetRepository(null, betState);
            var command = new AnswerBetCommand(memberId.Value, betId.Value, true, new FakeDateTimeProvider(dateTimeAnswerBet));
            var handler = new AnswerBetCommandHandler(memberRepository, betRepository, new InMemoryAuthenticationGateway(memberId.Value));

            //act
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            //assert
            Assert.IsType<MemberHasNotEnoughCoinsException>(record);
            Assert.Equal($"Member has not enough coins to bet. Wallet: {member.Wallet}, Required: {betState.Coins}",
                        record.Message);
        }

        [Fact]
        public async Task ShouldThrowNotAuthenticatedException()
        {
            var command = new AnswerBetCommand(Guid.NewGuid(), default, true, null);
            var handler = new AnswerBetCommandHandler(new InMemoryMemberRepository(), default, new InMemoryAuthenticationGateway(Guid.NewGuid()));

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<NotAuthenticatedException>(record);
        }
    }
}
