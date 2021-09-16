using BetFriend.Application.Abstractions.Command;
using System;

namespace BetFriend.Application.Usecases.UpdateWallet
{
    public class UpdateWalletMembersCommand : ICommand
    {
        public UpdateWalletMembersCommand(Guid betId)
        {
            BetId = betId;
        }

        internal Guid BetId { get; }
    }
}
