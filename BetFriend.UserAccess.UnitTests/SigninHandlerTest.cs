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
        [Fact]
        public async Task ShouldSignInUserIfIdentificationOK()
        {
            var command = new SignInCommand("username", "password");
            var presenter = new InMemorySignInPresenter();
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 24));
            var handler = new SignInCommandHandler(new InMemoryUserRepository(user), new InMemoryHashPassword("password"), new InMemoryTokenGenerator("jwtToken"), presenter);
            await handler.Handle(command, default);
            Assert.Equal("jwtToken", presenter.Token);
        }

        [Fact]
        public async Task ShouldThrowAuthenticationNotValidException()
        {
            var command = new SignInCommand("username", "password");
            var presenter = new InMemorySignInPresenter();
            var user = new User("abc", "usernam", "email@email.com", "password", new DateTime(2021, 9, 24));
            var handler = new SignInCommandHandler(new InMemoryUserRepository(user), new InMemoryHashPassword("password"), new InMemoryTokenGenerator("jwtToken"), presenter);
            var record = await Record.ExceptionAsync(() => handler.Handle(command, default));
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
