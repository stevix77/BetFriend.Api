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

        public RegisterController(IUserAccessProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterInput registerInput)
        {
            var command = new RegisterCommand(Guid.NewGuid().ToString(),
                                              registerInput.Username,
                                              registerInput.Password,
                                              registerInput.Email);
            var result = await _processor.ExecuteCommandAsync(command);
            return Created(string.Empty, result);
        }
    }
}
