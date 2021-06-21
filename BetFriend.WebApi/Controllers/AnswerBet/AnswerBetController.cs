namespace BetFriend.WebApi.Controllers.AnswerBet
{
    using BetFriend.Application.Abstractions;
    using BetFriend.Application.Usecases.AnswerBet;
    using BetFriend.Infrastructure.DateTimeProvider;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/answer")]
    public class AnswerBetController : Controller
    {
        private readonly IProcessor _processor;

        public AnswerBetController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpPost]
        public async Task<IActionResult> AnswerBet([FromBody] AnswerBetInput input)
        {
            var command = new AnswerBetCommand(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"), input.BetId, input.Answer, new DateTimeProvider());
            await _processor.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
