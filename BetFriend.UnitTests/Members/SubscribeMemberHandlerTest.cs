﻿namespace BetFriend.UnitTests.Members
{
    using BetFriend.Application.Usecases.SubscribeMember;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using BetFriend.Infrastructure.Repositories.InMemory;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class SubscribeMemberHandlerTest
    {
        [Fact]
        public async Task ShouldAddSubscription()
        {
            var subscriptionId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var memberToSubscribe = new Member(new(subscriptionId), "member2", 300);
            var member = new Member(new(memberId), "member1", 300);
            var memberReposiory = new InMemoryMemberRepository(new() { member, memberToSubscribe });
            var handler = new SubscribeMemberCommanderHandler(memberReposiory);
            var command = new SubscribeMemberCommand(memberId, subscriptionId);

            await handler.Handle(command, default);

            Assert.Collection(member.Subscriptions, x =>
            {
                Assert.Equal(subscriptionId, x.MemberId.Value);
            });
        }

        [Fact]
        public async Task ShouldNotSubscribeIfMemberToFollowDoesNotExist()
        {
            var memberIdToSubscribe = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var member = new Member(new(memberId), "member1", 300);
            var memberReposiory = new InMemoryMemberRepository(new() { member });
            var handler = new SubscribeMemberCommanderHandler(memberReposiory);
            var command = new SubscribeMemberCommand(memberId, memberIdToSubscribe);

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.DoesNotContain(member.Subscriptions, x => x.MemberId.Value == memberIdToSubscribe);
            Assert.IsType<MemberUnknownException>(record);
            Assert.Equal($"Member with id {memberIdToSubscribe} does not exist", record.Message);
        }

        [Fact]
        public async Task ShouldNotSubscribeIfMemberDoesNotExist()
        {
            var memberIdToSubscribe = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var member = new Member(new(memberIdToSubscribe), "member2", 300);
            var memberReposiory = new InMemoryMemberRepository(new() { member });
            var handler = new SubscribeMemberCommanderHandler(memberReposiory);
            var command = new SubscribeMemberCommand(memberId, memberIdToSubscribe);

            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));

            Assert.IsType<MemberUnknownException>(record);
            Assert.Equal($"Member with id {memberId} does not exist", record.Message);
        }

        [Fact]
        public async Task ShouldNotSubscribeIfAlreadyFollowed()
        {
            var memberIdToSubscribe = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            var member = new Member(new(memberId), "member1", 300);
            member.Subscribe(new(new(memberIdToSubscribe)));
            var memberToSubscribe = new Member(new(memberIdToSubscribe), "member2", 300);
            var memberReposiory = new InMemoryMemberRepository(new() { member, memberToSubscribe });
            var handler = new SubscribeMemberCommanderHandler(memberReposiory);
            var command = new SubscribeMemberCommand(memberId, memberIdToSubscribe);

            await handler.Handle(command, default);

            Assert.Equal(1, member.Subscriptions.Count);
        }
    }
}
