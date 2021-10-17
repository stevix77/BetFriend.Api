namespace BetFriend.Bet.Infrastructure.Extensions
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;
    using System.Linq;


    internal static class BetExtension
    {
        internal static Bet ToBet(this BetEntity entity)
        {
            return Bet.FromState(new BetState(entity.BetId,
                                            entity.Creator.ToMember(),
                                            entity.EndDate,
                                            entity.Description,
                                            entity.Coins,
                                            entity.CreationDate,
                                            entity.Answers?.Select(x =>
                                                new AnswerState(x.Member.ToMember(),
                                                                x.IsAccepted,
                                                                x.DateAnswer))
                                                            .ToList()));
        }
    }
}
