using BetFriend.UserAccess.Application.Usecases.Register;

namespace BetFriend.WebApi.Controllers.Register
{
    public class RegisterPresenter : IRegisterPresenter
    {
        public string ViewModel { get; private set; }

        public void Present(string token)
        {
            ViewModel = token;
        }
    }
}