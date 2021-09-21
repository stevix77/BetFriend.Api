using System;

namespace BetFriend.Bet.Domain
{
    public interface IAuthenticationGateway
    {
        bool IsAuthenticated(Guid memberId);
    }
}
