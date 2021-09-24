namespace BetFriend.UserAccess.Application.Usecases.SignIn
{
    using BetFriend.Shared.Application.Abstractions.Command;


    public class SignInCommand : ICommand
    {
        public SignInCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }
    }
}
