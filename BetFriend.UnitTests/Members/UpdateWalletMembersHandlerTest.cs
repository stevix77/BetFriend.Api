using BetFriend.Application;
using BetFriend.Application.Usecases.UpdateWallet;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Members;
using BetFriend.Infrastructure.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UnitTests.Members
{
    public class UpdateWalletMembersHandlerTest
    {
        [Fact]
        public async Task ShouldIncreaseWallet()
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
                                            new AnswerState(participant.Id.Value, true, new DateTime(2021, 3,4)),
                                            new AnswerState(participant2.Id.Value, true, new DateTime(2021, 3,4))
                                        },
                                        new Status(true, new DateTime(2021, 5, 5)));
            var domainEventListener = new DomainEventsListener();
            IBetRepository betRepository = new InMemoryBetRepository(domainEventListener, betState);
            IMemberRepository memberRepository = new InMemoryMemberRepository(new() { creator, participant, participant2 });
            var command = new UpdateWalletMembersCommand(betId);
            var handler = new UpdateWalletMembersCommandHandler(betRepository, memberRepository);

            await handler.Handle(command, default);

            var creatorUpdated = await memberRepository.GetByIdAsync(creator.Id);
            var participantUpdated = await memberRepository.GetByIdAsync(participant.Id);
            var participantUpdated2 = await memberRepository.GetByIdAsync(participant2.Id);
            Assert.Equal(250, creatorUpdated.Wallet);
            Assert.Equal(50, participantUpdated.Wallet);
            Assert.Equal(450, participantUpdated2.Wallet);
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
    }
}
