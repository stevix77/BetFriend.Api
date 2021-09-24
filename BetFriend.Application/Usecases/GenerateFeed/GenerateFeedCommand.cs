using BetFriend.Bet.Domain.Members;
using BetFriend.Shared.Application.Abstractions.Command;
using System;

namespace BetFriend.Bet.Application.Usecases.GenerateFeed
{
    public class GenerateFeedCommand : ICommand
    {
        public GenerateFeedCommand(MemberId memberId)
        {
            FeedId = memberId.ToString();
        }

        public string FeedId { get; }
    }
}
