namespace BetFriend.Bet.Infrastructure.Gateways
{
    using BetFriend.Shared.Domain;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    public class AuthenticationGateway : IAuthenticationGateway
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthenticationGateway(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _dateTimeProvider = dateTimeProvider;
        }

        private string _userId;
        public Guid UserId
        {
            get
            {
                if (Guid.TryParse(_userId, out Guid userId))
                    return userId;
                throw new Exception($"UserId is not available");
            }
        }

        public bool IsAuthenticated()
        {
            var token = GetTokenFromHeaders();
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            _userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var isValid = _dateTimeProvider.Now.CompareTo(jwtToken.ValidTo) == -1;
            return isValid;
        }

        private string GetTokenFromHeaders() => _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
    }
}
