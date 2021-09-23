namespace BetFriend.UserAccess.Infrastructure.TokenGenerator
{
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using System.Threading.Tasks;


    public class InMemoryTokenGenerator : ITokenGenerator
    {
        private readonly string _token;

        public InMemoryTokenGenerator(string token)
        {
            _token = token;
        }

        public Task<string> GenerateAsync(User user)
        {
            return Task.FromResult(_token);
        }
    }
}
