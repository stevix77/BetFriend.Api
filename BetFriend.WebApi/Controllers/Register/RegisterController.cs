namespace BetFriend.WebApi.Controllers.Register
{
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;


    [Route("api/users/register")]
    public class RegisterController : Controller
    {
        private readonly IUserAccessModule _module;

        public RegisterController(IUserAccessModule module)
        {
            _module = module;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterInput registerInput)
        {
            var command = new RegisterCommand(Guid.NewGuid().ToString(),
                                              registerInput.Username,
                                              registerInput.Password,
                                              registerInput.Email);
            var result = await _module.ExecuteCommandAsync(command);
            return Created(string.Empty, result);
        }
    }
}
