namespace BetFriend.Bet.Application.Usecases.InsertBetQuerySide
{
    using MediatR;
    using System;


    public class InsertBetQuerySideNotification : INotification
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
