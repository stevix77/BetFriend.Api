namespace BetFriend.Bet.Application.Usecases.SubscribeMember
{
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;


    public sealed class SubscribeMemberCommand : ICommand
    {
        private Guid _subscriptionId;

        public SubscribeMemberCommand(Guid subscriptionId)
        {
            _subscriptionId = subscriptionId;
        }

        public MemberId SubscriptionId { get => new(_subscriptionId); }
    }
}
