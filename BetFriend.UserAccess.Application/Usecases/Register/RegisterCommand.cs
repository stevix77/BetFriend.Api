namespace BetFriend.UserAccess.Application.Usecases.Register
{
    using BetFriend.Shared.Application.Abstractions.Command;

    public sealed class RegisterCommand : ICommand
    {
        public RegisterCommand(string userId, string username, string password, string email)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Email = email;
        }

        public string UserId { get; }
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
    }
}
