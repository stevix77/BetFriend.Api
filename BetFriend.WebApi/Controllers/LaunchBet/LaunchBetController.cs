using BetFriend.Application.Abstractions;
using BetFriend.Application.Usecases.LaunchBet;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BetFriend.WebApi.Controllers.LaunchBet
{
    [Route("api/bets/launch")]
    public class LaunchBetController : Controller
    {
        private readonly IProcessor _processor;

        public LaunchBetController(IProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> LaunchBet([FromBody] LaunchBetInput input)
        {
            var command = new LaunchBetCommand(input.BetId, Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"), input.EndDate, input.Coins, input.Description);
            await _processor.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
