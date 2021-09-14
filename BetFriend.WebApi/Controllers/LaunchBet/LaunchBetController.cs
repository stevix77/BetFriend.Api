using BetFriend.Application.Abstractions;
using BetFriend.Application.Usecases.LaunchBet;
using BetFriend.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BetFriend.WebApi.Controllers.LaunchBet
{
    [Route("api/bets/launch")]
    public class LaunchBetController : Controller
    {
        private readonly IProcessor _processor;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LaunchBetController(IProcessor processor, IDateTimeProvider dateTimeProvider)
        {
            _processor = processor;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpPost]
        public async Task<IActionResult> LaunchBet([FromBody] LaunchBetInput input)
        {
            var command = new LaunchBetCommand(input.BetId, Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"), input.EndDate, input.Coins, input.Description, _dateTimeProvider);
            await _processor.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
