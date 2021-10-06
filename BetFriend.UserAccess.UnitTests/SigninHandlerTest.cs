namespace BetFriend.UserAccess.UnitTests
{
    using BetFriend.UserAccess.Application.Usecases.SignIn;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using BetFriend.UserAccess.Infrastructure.TokenGenerator;
    using System;
    using System.Threading.Tasks;
    using Xunit;


    public class SigninHandlerTest
    {
        [Fact]
        public async Task ShouldSignInUserIfIdentificationOK()
        {
            var command = new SignInCommand("username", "password");
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 24));
            AssertThatHandlerReturnToken(await SignIn(command, user));
        }

        [Fact]
        public async Task ShouldSignInUserIfIdentificationOKWithEmail()
        {
            var command = new SignInCommand("email@email.com", "password");
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 24));
            var result = await SignIn(command, user);
            AssertThatHandlerReturnToken(result);
        }

        [Fact]
        public async void ShouldThrowAuthenticationNotValidException()
        {
            var command = new SignInCommand("username", "password");
            var user = new User("abc", "usernam", "email@email.com", "password", new DateTime(2021, 9, 24));
            await AssertThatAuthenticationIsNotValid(command, user);
        }


        private void AssertThatHandlerReturnToken(string token)
        {
            Assert.Equal("jwtToken", token);
        }

        private async Task<string> SignIn(SignInCommand command, User user)
        {
            var handler = new SignInCommandHandler(new InMemoryUserRepository(user),
                                                   new InMemoryHashPassword("password"),
                                                   new InMemoryTokenGenerator("jwtToken"));
            return await handler.Handle(command, default);
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
}
