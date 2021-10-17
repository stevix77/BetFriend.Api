using System;
using System.Runtime.Serialization;

namespace BetFriend.Bet.Domain.Exceptions
{
    [Serializable]
    public class MemberHasNotEnoughCoinsException : Exception
    {
        public MemberHasNotEnoughCoinsException(string message) : base(message)
        {
        }

        public MemberHasNotEnoughCoinsException(decimal wallet, int coins)
        {
            throw new MemberHasNotEnoughCoinsException($"Member has not enough coins to bet. Wallet: {wallet}, Required: {coins}");
        }
    }
}
