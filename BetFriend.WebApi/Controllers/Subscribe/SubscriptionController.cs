namespace BetFriend.WebApi.Controllers.Subscribe
{
    using BetFriend.Bet.Application.Usecases.SubscribeMember;
    using BetFriend.Shared.Application.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/subscriptions/{subscriptionId}")]
    public class SubscriptionController : Controller
    {
        private readonly IProcessor _processor;

        public SubscriptionController(IProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromRoute] Guid subscriptionId)
        {
            var command = new SubscribeMemberCommand(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1"), subscriptionId);
            await _processor.ExecuteCommandAsync(command).ConfigureAwait(false);
            return Ok();
        }
    }
}
