namespace BetFriend.Bet.Infrastructure.Gateways
{
    using BetFriend.Shared.Domain;
    using Microsoft.AspNetCore.Http;
    using System;


    public class AuthenticationGateway : IAuthenticationGateway
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationGateway(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated(Guid memberId)
        {
            throw new NotImplementedException();
        }
    }
}
