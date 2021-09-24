using BetFriend.UserAccess.Application.Usecases.SignIn;
using BetFriend.UserAccess.Domain;
using BetFriend.UserAccess.Domain.Exceptions;
using BetFriend.UserAccess.Domain.Users;
using BetFriend.UserAccess.Infrastructure.Hash;
using BetFriend.UserAccess.Infrastructure.Repositories;
using BetFriend.UserAccess.Infrastructure.TokenGenerator;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UserAccess.UnitTests
{
    /// <summary>
    /// given a user send right username with password
    /// when he signin 
    /// then he receive a token 
    /// 
    /// given a user send a bad username
    /// when he signin
    /// then he throw a AuthenticationNotValidException
    /// 
    /// /// given a user send a bad password
    /// when he signin
    /// then he throw a AuthenticationNotValidException
    /// 
    /// </summary>
    public class SigninHandlerTest
    {
        private readonly InMemorySignInPresenter _presenter = new InMemorySignInPresenter();
        
        [Fact]
        public async Task ShouldSignInUserIfIdentificationOK()
        {
            var command = new SignInCommand("username", "password");
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 24));
            await SignIn(command, user);
            AssertThatPresenterHasAToken();
        }

        [Fact]
        public async Task ShouldSignInUserIfIdentificationOKWithEmail()
        {
            var command = new SignInCommand("email@email.com", "password");
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 24));
            await SignIn(command, user);
            AssertThatPresenterHasAToken();
        }

        [Fact]
        public async void ShouldThrowAuthenticationNotValidException()
        {
            var command = new SignInCommand("username", "password");
            var user = new User("abc", "usernam", "email@email.com", "password", new DateTime(2021, 9, 24));
            await AssertThatAuthenticationIsNotValid(command, user);
        }


        private void AssertThatPresenterHasAToken()
        {
            Assert.Equal("jwtToken", _presenter.Token);
        }

        private async Task SignIn(SignInCommand command, User user)
        {
            var handler = new SignInCommandHandler(new InMemoryUserRepository(user),
                                                   new InMemoryHashPassword("password"),
                                                   new InMemoryTokenGenerator("jwtToken"),
                                                   _presenter);
            await handler.Handle(command, default);
        }

        private async Task AssertThatAuthenticationIsNotValid(SignInCommand command, User user)
        {
            var record = await Record.ExceptionAsync(() => SignIn(command, user));
            Assert.IsType<AuthenticationNotValidException>(record);
        }
    }

    internal class InMemoryHashPassword : IHashPassword
    {
        private string _hashedPassword;

        public InMemoryHashPassword(string hashedPassword)
        {
            _hashedPassword = hashedPassword;
        }

        public string Hash(string password)
        {
            return _hashedPassword;
        }
    }

    internal class InMemorySignInPresenter : ISignInPresenter
    {
        public InMemorySignInPresenter()
        {
        }

        public string Token { get; private set; }

        public void Present(string token)
        {
            Token = token;
        }
    }
}
