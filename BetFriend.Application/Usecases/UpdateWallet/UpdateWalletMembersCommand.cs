namespace BetFriend.Bet.Application.Usecases.UpdateWallet
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using System;


    public class UpdateWalletMembersCommand : ICommand
    {
        public UpdateWalletMembersCommand(Guid betId)
        {
            BetId = betId;
        }

        internal Guid BetId { get; }
    }
}
