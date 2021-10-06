namespace BetFriend.WebApi.Controllers.Register
{
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;


    [Route("api/users/register")]
    public class RegisterController : Controller
    {
        private readonly IUserAccessProcessor _processor;
        private readonly IRegisterPresenter _registerPresenter;

        public RegisterController(IUserAccessProcessor processor, IRegisterPresenter registerPresenter)
        {
            _processor = processor;
            _registerPresenter = registerPresenter;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterInput registerInput)
        {
            var command = new RegisterCommand(Guid.NewGuid().ToString(),
                                              registerInput.Username,
                                              registerInput.Password,
                                              registerInput.Email);
            await _processor.ExecuteCommandAsync(command);
            return Created(string.Empty, null);
        }
    }
}
