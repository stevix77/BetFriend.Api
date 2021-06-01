using Microsoft.AspNetCore.Mvc;

namespace BetFriend.WebApi.Controllers.LaunchBet
{
    [Route("api/bets/launch")]
    public class LaunchBetController : Controller
    {
        public LaunchBetController()
        {
        }

        [HttpPost]
        public IActionResult LaunchBet([FromBody] LaunchBetInput input)
        {
            return View();
        }
    }
}
