using System;

namespace BetFriend.Shared.Domain
{
    public interface IAuthenticationGateway
    {
        bool IsAuthenticated(Guid memberId);
    }
}
