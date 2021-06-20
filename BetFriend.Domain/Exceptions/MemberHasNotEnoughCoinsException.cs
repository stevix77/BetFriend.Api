using System;
using System.Runtime.Serialization;

namespace BetFriend.Domain.Exceptions
{
    [Serializable]
    public class MemberHasNotEnoughCoinsException : Exception
    {
        public MemberHasNotEnoughCoinsException(string message) : base(message)
        {
        }

        public MemberHasNotEnoughCoinsException(int wallet, int coins)
        {
            throw new MemberHasNotEnoughCoinsException($"Member has not enough coins to bet. Wallet: {wallet}, Required: {coins}");
        }
    }
}
