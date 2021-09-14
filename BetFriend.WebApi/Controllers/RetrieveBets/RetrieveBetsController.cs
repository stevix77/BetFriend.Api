namespace BetFriend.WebApi.Controllers.RetrieveBets
{
    using BetFriend.Application.Abstractions;
    using BetFriend.Application.Usecases.RetrieveBets;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/bets")]
    public class RetrieveBetsController : Controller
    {
        private readonly IProcessor _processor;

        public RetrieveBetsController(IProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        public async Task<IActionResult> Retrieve()
        {
            var query = new RetrieveBetsQuery(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"));
            var betsDto = await _processor.ExecuteQueryAsync(query);
            return Ok(betsDto);
        }
    }
}
