namespace BetFriend.WebApi.Controllers.LaunchBet
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.LaunchBet;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;


    [Route("api/bets/launch")]
    public class LaunchBetController : Controller
    {
        private readonly IBetModule _module;

        public LaunchBetController(IBetModule module)
        {
            _module = module;
        }

        [HttpPost]
        public async Task<IActionResult> LaunchBet([FromBody] LaunchBetInput input)
        {
            var command = new LaunchBetCommand(input.BetId, input.EndDate, input.Coins, input.Description);
            await _module.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
