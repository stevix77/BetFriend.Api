namespace BetFriend.WebApi.Controllers.Subscribe
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.SubscribeMember;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/subscriptions/{subscriptionId}")]
    public class SubscriptionController : Controller
    {
        private readonly IBetModule _module;

        public SubscriptionController(IBetModule module)
        {
            _module = module;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromRoute] Guid subscriptionId)
        {
            var command = new SubscribeMemberCommand(subscriptionId);
            await _module.ExecuteCommandAsync(command).ConfigureAwait(false);
            return Ok();
        }
    }
}
