using System.Threading.Tasks;

namespace BetFriend.Bet.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit();
        Task Rollback();
    }
}
