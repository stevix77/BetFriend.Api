namespace BetFriend.Bet.Infrastructure.Extensions
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;
    using System;
    using System.Linq;


    internal static class BetExtension
    {
        internal static Bet ToBet(this BetEntity entity)
        {
            var betState = new BetState(entity.BetId,
                                        entity.Creator.ToMember(),
                                        entity.EndDate,
                                        entity.Description,
                                        entity.Coins,
                                        entity.CreationDate,
                                        entity.Answers?.Select(x =>
                                            new AnswerState(x.Member.ToMember(),
                                                            x.IsAccepted,
                                                            x.DateAnswer))
                                                        .ToList(),
                                        entity.CloseDate,
                                        entity.IsSuccess);
            var bet = Bet.FromState(betState);
            return bet;
        }
    }
}
