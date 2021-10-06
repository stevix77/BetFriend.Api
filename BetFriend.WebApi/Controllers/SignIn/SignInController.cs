namespace BetFriend.WebApi.Controllers.SignIn
{
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.SignIn;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;


    [Route("api/users/signin")]
    public class SignInController : Controller
    {
        private readonly IUserAccessProcessor _processor;

        public SignInController(IUserAccessProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInInput signInInput)
        {
            var command = new SignInCommand(signInInput.Login,
                                            signInInput.Password);
            var result = await _processor.ExecuteCommandAsync(command);
            return Ok(result);
        }
    }
}
