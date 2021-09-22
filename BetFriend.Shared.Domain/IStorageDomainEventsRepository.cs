namespace BetFriend.Shared.Domain
{
    using System.Threading.Tasks;

    public interface IStorageDomainEventsRepository
    {
        Task SaveAsync(IDomainEvent item);
    }
}
