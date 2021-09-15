﻿namespace BetFriend.Application.Usecases.CloseBet
{
    using BetFriend.Application.Abstractions.Command;
    using System;


    public sealed class CloseBetCommand : ICommand
    {
        public Guid MemberId { get; }
        public Guid BetId { get; }

        public bool Success { get; }

        public CloseBetCommand(Guid betId, Guid value, bool success)
        {
            BetId = betId;
            MemberId = value;
            Success = success;
        }
    }
}