namespace BetFriend.WebApi.Controllers.Register
{
    using BetFriend.UserAccess.Application.Usecases.Register;


    public class RegisterPresenter : IRegisterPresenter
    {
        public string ViewModel { get; internal set; }

        public void Present(string token)
        {
            ViewModel = token;
        }
    }
}