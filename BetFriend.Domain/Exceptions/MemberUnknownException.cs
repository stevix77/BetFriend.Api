namespace BetFriend.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MemberUnknownException : Exception
    {
        public MemberUnknownException()
        {
        }

        public MemberUnknownException(string message) : base(message)
        {
        }

        public MemberUnknownException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MemberUnknownException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
