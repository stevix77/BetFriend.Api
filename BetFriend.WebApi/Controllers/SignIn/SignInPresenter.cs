namespace BetFriend.WebApi.Controllers.SignIn
{
    using BetFriend.UserAccess.Application.Usecases.SignIn;


    public class SignInPresenter : ISignInPresenter
    {
        public string ViewModel { get; private set; }

        public void Present(string token)
        {
            ViewModel = token;
        }
    }
}