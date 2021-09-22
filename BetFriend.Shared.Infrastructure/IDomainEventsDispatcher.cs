namespace BetFriend.Shared.Infrastructure
{
    using System.Threading.Tasks;

    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}
