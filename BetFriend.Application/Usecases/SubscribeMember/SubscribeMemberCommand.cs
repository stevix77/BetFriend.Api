namespace BetFriend.Application.Usecases.SubscribeMember
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Domain.Members;
    using System;


    public sealed class SubscribeMemberCommand : ICommand
    {
        private Guid _memberId;
        private Guid _subscriptionId;

        public SubscribeMemberCommand(Guid memberId, Guid subscriptionId)
        {
            _memberId = memberId;
            _subscriptionId = subscriptionId;
        }

        public MemberId MemberId { get => new(_memberId); }
        public MemberId SubscriptionId { get => new(_subscriptionId); }
    }
}
