using System;
using System.Runtime.Serialization;

namespace BetFriend.Domain.Exceptions
{
    [Serializable]
    public class MemberDoesNotEnoughTokensException : Exception
    {
        public MemberDoesNotEnoughTokensException()
        {
        }

        public MemberDoesNotEnoughTokensException(string message) : base(message)
        {
        }

        public MemberDoesNotEnoughTokensException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MemberDoesNotEnoughTokensException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
