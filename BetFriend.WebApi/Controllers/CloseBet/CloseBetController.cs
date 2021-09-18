namespace BetFriend.WebApi.Controllers.CloseBet
{
    using BetFriend.Application.Abstractions;
    using BetFriend.Application.Usecases.CloseBet;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/{betId}/close")]
    public class CloseBetController : Controller
    {
        private readonly IProcessor _processor;

        public CloseBetController(IProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> CloseBet([FromRoute] Guid betId, [FromBody] bool isSuccess)
        {
            var command = new CloseBetCommand(betId,
                                              Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"),
                                              isSuccess);
            await _processor.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
