namespace BetFriend.WebApi.Controllers.AnswerBet
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.AnswerBet;
    using BetFriend.Shared.Domain;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/answer")]
    public class AnswerBetController : Controller
    {
        private readonly IBetModule _module;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AnswerBetController(IBetModule processor, IDateTimeProvider dateTimeProvider)
        {
            _module = processor ?? throw new ArgumentNullException(nameof(processor));
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpPost]
        public async Task<IActionResult> AnswerBet([FromBody] AnswerBetInput input)
        {
            var command = new AnswerBetCommand(input.BetId, input.Answer);
            await _module.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
