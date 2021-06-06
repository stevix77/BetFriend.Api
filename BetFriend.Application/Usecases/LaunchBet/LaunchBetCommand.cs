﻿namespace BetFriend.Application.Usecases.LaunchBet
{
    using BetFriend.Application.Abstractions.Command;
    using System;

    public class LaunchBetCommand : ICommand
    {
        public LaunchBetCommand(Guid betId, Guid memberId, DateTime endDate, int tokens, string description)
        {
            BetId = betId;
            CreatorId = memberId;
            EndDate = endDate;
            Coins = tokens;
            Description = description;
        }

        public Guid BetId { get; }
        public Guid CreatorId { get; }
        public DateTime EndDate { get; }
        public int Coins { get; }
        public string Description { get; }
    }
}
