namespace BetFriend.WebApi.Controllers.SignIn
{
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.SignIn;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;


    [Route("api/users/signin")]
    public class SignInController : Controller
    {
        private readonly IProcessor _processor;
        private readonly SignInPresenter _signInPresenter;

        public SignInController(IProcessor processor, SignInPresenter signInPresenter)
        {
            _processor = processor;
            _signInPresenter = signInPresenter;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SignInInput signInInput)
        {
            var command = new SignInCommand(signInInput.Login,
                                            signInInput.Password);
            await _processor.ExecuteCommandAsync(command);
            return Created(string.Empty, _signInPresenter.ViewModel);
        }
    }
}
