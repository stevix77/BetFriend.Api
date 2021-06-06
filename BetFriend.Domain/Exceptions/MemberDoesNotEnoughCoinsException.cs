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

        public MemberDoesNotEnoughCoinsException(string message) : base(message)
        {
        }

        public MemberDoesNotEnoughCoinsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MemberDoesNotEnoughCoinsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
