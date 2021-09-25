using BetFriend.Bet.Application.Models;
using System.Threading.Tasks;

namespace BetFriend.Bet.Application.Abstractions.Repository
{
    public interface IFeedQueryRepository
    {
        Task SaveAsync(FeedDto feedDto);
    }
}
