using System.Threading.Tasks;

namespace BetFriend.Shared.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit();
        Task Rollback();
    }
}
