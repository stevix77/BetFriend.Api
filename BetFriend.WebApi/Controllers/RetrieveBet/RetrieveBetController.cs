namespace BetFriend.WebApi.Controllers.RetrieveBet
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Application.Usecases.RetrieveBet;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/{betId}")]
    public class RetrieveBetController : Controller
    {
        private readonly IProcessor _processor;

        public RetrieveBetController(IProcessor processor)
        {
            _processor = processor;
        }

        [ProducesResponseType(200, Type = typeof(BetDto))]
        [HttpGet]
        public async Task<IActionResult> Retrieve([FromRoute] Guid betId)
        {
            var query = new RetrieveBetQuery(betId);
            var betDto = await _processor.ExecuteQueryAsync(query);
            return Ok(betDto);
        }
    }
}
