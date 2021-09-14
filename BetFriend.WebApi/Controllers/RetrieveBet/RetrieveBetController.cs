namespace BetFriend.WebApi.Controllers.RetrieveBet
{
    using BetFriend.Application.Abstractions;
    using BetFriend.Application.Usecases.RetrieveBet;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/{betId}")]
    public class RetrieveBetsController : Controller
    {
        private readonly IProcessor _processor;

        public RetrieveBetsController(IProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        public async Task<IActionResult> Retrieve([FromRoute] Guid betId)
        {
            var query = new RetrieveBetQuery(betId);
            var betDto = await _processor.ExecuteQueryAsync(query);
            return Ok(betDto);
        }
    }
}
