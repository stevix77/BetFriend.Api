namespace BetFriend.UserAccess.Infrastructure.TokenGenerator
{
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using System.Threading.Tasks;


    public sealed class JwtTokenGenerator : ITokenGenerator
    {
        public JwtTokenGenerator()
        {
        }

        public Task<string> GenerateAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
