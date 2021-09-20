namespace BetFriend.Bet.Infrastructure
{
    using System.Threading.Tasks;

    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}
