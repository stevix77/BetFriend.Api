namespace BetFriend.Bet.Application.Usecases.InsertBetQuerySide
{
    using BetFriend.Shared.Application.Abstractions.Notification;
    using System;


    public class InsertBetQuerySideNotification : INotificationCommand
    {
        public Guid BetId { get; }
        public Guid MemberId { get; }

        public InsertBetQuerySideNotification(Guid betId, Guid memberId)
        {
            BetId = betId;
            MemberId = memberId;
        }
    }
}
