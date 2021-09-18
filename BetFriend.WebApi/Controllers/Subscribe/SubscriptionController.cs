﻿using BetFriend.Application.Abstractions;
using BetFriend.Application.Usecases.SubscribeMember;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BetFriend.WebApi.Controllers.Subscribe
{
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