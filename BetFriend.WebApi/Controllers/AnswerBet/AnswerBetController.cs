namespace BetFriend.WebApi.Controllers.AnswerBet
{
    using BetFriend.Bet.Application.Usecases.AnswerBet;
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.Shared.Domain;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/answer")]
    public class AnswerBetController : Controller
    {
        private readonly IProcessor _processor;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AnswerBetController(IProcessor processor, IDateTimeProvider dateTimeProvider)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpPost]
        public async Task<IActionResult> AnswerBet([FromBody] AnswerBetInput input)
        {
            var command = new AnswerBetCommand(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"), input.BetId, input.Answer, _dateTimeProvider);
            await _processor.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
