using System;

namespace BetFriend.Shared.Domain
{
    public interface IAuthenticationGateway
    {
        Guid UserId { get; }

        bool IsAuthenticated();
    }
}
