namespace BetFriend.WebApi.Controllers.CloseBet
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.CloseBet;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/{betId}/close")]
    public class CloseBetController : Controller
    {
        private readonly IBetModule _module;

        public CloseBetController(IBetModule module)
        {
            _module = module;
        }

        [HttpPost]
        public async Task<IActionResult> CloseBet([FromRoute] Guid betId, [FromBody] bool isSuccess)
        {
            var command = new CloseBetCommand(betId,
                                              isSuccess);
            await _module.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
