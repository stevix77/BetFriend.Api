using System.Threading.Tasks;

namespace BetFriend.Infrastructure
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();
    }
}
