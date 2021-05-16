namespace BetFriend.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EndDateNotValidException : Exception
    {
        public EndDateNotValidException()
        {
        }

        public EndDateNotValidException(string message) : base(message)
        {
        }

        public EndDateNotValidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EndDateNotValidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
