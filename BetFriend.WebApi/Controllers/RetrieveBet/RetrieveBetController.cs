namespace BetFriend.WebApi.Controllers.RetrieveBet
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Application.Usecases.RetrieveBet;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets/{betId}")]
    public class RetrieveBetController : Controller
    {
        private readonly IBetModule _module;

        public RetrieveBetController(IBetModule module)
        {
            _module = module;
        }

        [ProducesResponseType(200, Type = typeof(BetDto))]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Bets" })]
        public async Task<IActionResult> Retrieve([FromRoute] Guid betId)
        {
            var query = new RetrieveBetQuery(betId);
            var betDto = await _module.ExecuteQueryAsync(query);
            return Ok(betDto);
        }
    }
}
