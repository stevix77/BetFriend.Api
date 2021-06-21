namespace BetFriend.WebApi.Controllers.AnswerBet
{
    using System;

    public class AnswerBetInput
    {
        public Guid BetId { get; set; }
        public bool Answer { get; set; }
    }
}