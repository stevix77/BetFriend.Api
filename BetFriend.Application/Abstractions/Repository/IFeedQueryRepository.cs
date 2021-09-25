namespace BetFriend.Bet.Application.Abstractions.Repository
{
    using BetFriend.Bet.Application.Models;
    using System.Threading.Tasks;


    public interface IFeedQueryRepository
    {
        Task SaveAsync(FeedDto feedDto);
    }
}
