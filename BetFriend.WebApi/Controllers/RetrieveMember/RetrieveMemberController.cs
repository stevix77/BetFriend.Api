using BetFriend.Bet.Application.Abstractions;
using BetFriend.Bet.Application.Models;
using BetFriend.Bet.Application.Usecases.RetrieveMember;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace BetFriend.WebApi.Controllers.RetrieveMember
{
    [Route("api/members")]
    public class RetrieveMemberController : Controller
    {
        private readonly IBetModule _betModule;

        public RetrieveMemberController(IBetModule betModule)
        {
            _betModule = betModule;
        }

        [HttpGet("{memberId}")]
        [ProducesResponseType(200, Type = typeof(MemberDto))]
        [SwaggerOperation(Tags = new [] {"Members"})]
        public async Task<IActionResult> Retrieve(Guid memberId)
        {
            var member = await _betModule.ExecuteQueryAsync(new RetrieveMemberQuery(memberId));
            return member is null ? NotFound(new ProblemDetails
            {
                Detail = $"Member with id {memberId} is not found",
                Title = "NotFound"
            }) : Ok(member);
        }
    }
}
