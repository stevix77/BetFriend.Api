using BetFriend.Bet.Application.Usecases.UpdateWallet;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Exceptions;
using BetFriend.Bet.Domain.Members;
using BetFriend.Bet.Infrastructure.Repositories.InMemory;
using BetFriend.Shared.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.Bet.UnitTests.Members
{
    public class UpdateWalletMembersHandlerTest
    {
        [Fact]
        public async Task ShouldUpdateWallets()
        {
            var betId = Guid.NewGuid();
            var creator = new Member(new(Guid.NewGuid()), "creator", 200);
            var participant = new Member(new(Guid.NewGuid()), "participant", 100);
            var participant2 = new Member(new(Guid.NewGuid()), "participant2", 500);
            var betState = new BetState(betId,
                                        creator,
                                        new DateTime(2021, 12, 3),
                                        "description",
                                        50,
                                        new DateTime(2020, 3, 3),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(participant, true, new DateTime(2021, 3,4)),
                                            new AnswerState(participant2, true, new DateTime(2021, 3,4))
                                        }, new DateTime(2021, 11, 3), true);
            var domainEventListener = new DomainEventsAccessor();
            IBetRepository betRepository = new InMemoryBetRepository(domainEventListener, betState);
            IMemberRepository memberRepository = new InMemoryMemberRepository(new() { creator, participant, participant2 });
            var command = new UpdateWalletMembersCommand(betId);
            var handler = new UpdateWalletMembersCommandHandler(betRepository, memberRepository);

            await handler.Handle(command, default);

            var creatorUpdated = await memberRepository.GetByIdAsync(creator.Id);
            var participantUpdated = await memberRepository.GetByIdAsync(participant.Id);
            var participantUpdated2 = await memberRepository.GetByIdAsync(participant2.Id);
            Assert.Equal(250, creatorUpdated.Wallet);
            Assert.Equal(75, participantUpdated.Wallet);
            Assert.Equal(475, participantUpdated2.Wallet);
        }

        [Fact]
        public async Task ShouldThrowBetUnknownExceptionIfBetIdUnknown()
        {
            var betRepository = new InMemoryBetRepository();
            var betId = Guid.NewGuid();
            var memberRepository = new InMemoryMemberRepository();
            var command = new UpdateWalletMembersCommand(betId);
            var handler = new UpdateWalletMembersCommandHandler(betRepository, memberRepository);

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<BetUnknownException>(record);
            Assert.Equal($"This bet with id {betId} is unknown", record.Message);
        }

        [Fact]
        public async Task ShouldDecreaseWalletCreatorAndIncreaseParticipants()
        {
            var betId = Guid.NewGuid();
            var creator = new Member(new(Guid.NewGuid()), "creator", 200);
            var participant = new Member(new(Guid.NewGuid()), "participant", 100);
            var participant2 = new Member(new(Guid.NewGuid()), "participant2", 500);
            var betState = new BetState(betId,
                                        creator,
                                        new DateTime(2021, 12, 3),
                                        "description",
                                        50,
                                        new DateTime(2020, 3, 3),
                                        new List<AnswerState>()
                                        {
                                            new AnswerState(participant, true, new DateTime(2021, 3,4)),
                                            new AnswerState(participant2, true, new DateTime(2021, 3,4))
                                        }, new DateTime(2021, 3, 3), false);
            var domainEventListener = new DomainEventsAccessor();
            IBetRepository betRepository = new InMemoryBetRepository(domainEventListener, betState);
            IMemberRepository memberRepository = new InMemoryMemberRepository(new() { creator, participant, participant2 });
            var command = new UpdateWalletMembersCommand(betId);
            var handler = new UpdateWalletMembersCommandHandler(betRepository, memberRepository);

            await handler.Handle(command, default);

            var creatorUpdated = await memberRepository.GetByIdAsync(creator.Id);
            var participantUpdated = await memberRepository.GetByIdAsync(participant.Id);
            var participantUpdated2 = await memberRepository.GetByIdAsync(participant2.Id);
            Assert.Equal(150, creatorUpdated.Wallet);
            Assert.Equal(125, participantUpdated.Wallet);
            Assert.Equal(525, participantUpdated2.Wallet);
        }

        [Fact]
        public async Task ShouldNotModifyWalletCreatorWhenThereAreNoParticipants()
        {
            var betId = Guid.NewGuid();
            var creator = new Member(new(Guid.NewGuid()), "creator", 200);
            var betState = new BetState(betId,
                                        creator,
                                        new DateTime(2021, 12, 3),
                                        "description",
                                        50,
                                        new DateTime(2020, 3, 3),
                                        new List<AnswerState>(), 
                                        new DateTime(2021, 3, 3), true);
            var domainEventListener = new DomainEventsAccessor();
            var betRepository = new InMemoryBetRepository(domainEventListener, betState);
            var memberRepository = new InMemoryMemberRepository(new() { creator });
            var command = new UpdateWalletMembersCommand(betId);
            var handler = new UpdateWalletMembersCommandHandler(betRepository, memberRepository);

            await handler.Handle(command, default);

            Assert.Equal(200, creator.Wallet);
        }
    }
}
