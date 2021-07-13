namespace BetFriend.Application.Usecases.InsertBetQuerySide
{
    using BetFriend.Application.Abstractions.Command;
    using System;


    public class InsertBetQuerySideCommand : ICommand
    {
        public Guid BetId { get; }
        public Guid MemberId { get; }

        public InsertBetQuerySideCommand(Guid betId, Guid memberId)
        {
            BetId = betId;
            MemberId = memberId;
        }
    }
}
