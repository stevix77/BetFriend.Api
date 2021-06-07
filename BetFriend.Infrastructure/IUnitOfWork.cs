using System.Threading.Tasks;

namespace BetFriend.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit();
        Task Rollback();
    }
}
