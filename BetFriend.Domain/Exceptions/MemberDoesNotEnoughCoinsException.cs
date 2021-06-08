using System;
using System.Runtime.Serialization;

namespace BetFriend.Domain.Exceptions
{
    [Serializable]
    public class MemberDoesNotEnoughCoinsException : Exception
    {
        public MemberDoesNotEnoughCoinsException()
        {
        }
    }
}
