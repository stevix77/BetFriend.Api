namespace BetFriend.WebApi.Controllers.SignIn
{
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.SignIn;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Threading.Tasks;


    [Route("api/users/signin")]
    public class SignInController : Controller
    {
        private readonly IUserAccessModule _module;

        public SignInController(IUserAccessModule module)
        {
            _module = module;
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Users" })]
        public async Task<IActionResult> SignIn([FromBody] SignInInput signInInput)
        {
            var command = new SignInCommand(signInInput.Login,
                                            signInInput.Password);
            var result = await _module.ExecuteCommandAsync(command);
            return Ok(result);
        }
    }
}
