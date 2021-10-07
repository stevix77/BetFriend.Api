namespace BetFriend.UserAccess.Infrastructure.TokenGenerator
{
    using BetFriend.Shared.Domain;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.Configurations;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;


    public sealed class JwtTokenGenerator : ITokenGenerator
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtTokenGenerator(AuthenticationConfiguration authenticationConfiguration, IDateTimeProvider dateTimeProvider)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<string> GenerateAsync(User user)
        {
            string mySecret = _authenticationConfiguration.Secret;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = _dateTimeProvider.Now.AddDays(1),
                Issuer = _authenticationConfiguration.Issuer,
                Audience = _authenticationConfiguration.Audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
