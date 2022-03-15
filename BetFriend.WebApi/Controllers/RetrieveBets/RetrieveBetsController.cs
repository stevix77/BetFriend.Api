namespace BetFriend.WebApi.Controllers.RetrieveBets
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Application.Usecases.RetrieveBets;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/bets")]
    public class RetrieveBetsController : Controller
    {
        private readonly IBetModule _module;

        public RetrieveBetsController(IBetModule module)
        {
            _module = module;
        }

        [ProducesResponseType(200, Type = typeof(List<BetDto>))]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Bets" })]
        public async Task<IActionResult> Retrieve()
        {
            var query = new RetrieveBetsQuery();
            var betsDto = await _module.ExecuteQueryAsync(query);
            return Ok(betsDto);
        }
    }
}
