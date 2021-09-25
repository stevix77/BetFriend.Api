using BetFriend.Bet.Domain.Members;
using BetFriend.Shared.Application.Abstractions.Command;
using System;

namespace BetFriend.Bet.Application.Usecases.GenerateFeed
{
    public class GenerateFeedCommand : ICommand
    {
        public GenerateFeedCommand(MemberId memberId)
        {
            MemberId = memberId.ToString();
        }

        public string MemberId { get; }
    }
}
