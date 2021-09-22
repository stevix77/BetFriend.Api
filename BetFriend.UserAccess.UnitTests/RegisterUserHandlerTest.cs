namespace BetFriend.UserAccess.UnitTests
{
    using BetFriend.Shared.Infrastructure.DateTimeProvider;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class RegisterUserHandlerTest
    {
        private readonly InMemoryUserRepository repository;
        private readonly FakeDateTimeProvider fakeDateTimeProvider;
        
        public RegisterUserHandlerTest()
        {
            repository = new InMemoryUserRepository();
            fakeDateTimeProvider = new FakeDateTimeProvider(new DateTime(2021, 9, 22));
        }
        [Fact]
        public async Task ShouldRegisterAUser()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            await RegisterUser(command);
            AssertThatUserIsRegistered();
        }

        private void AssertThatUserIsRegistered()
        {
            var user = new User("abc", "username", "password", "email@email.com", new DateTime(2021, 9, 22));
            Assert.Equal(user, repository.GetUsers().Single());
        }

        private async Task RegisterUser(RegisterCommand command)
        {
            await new RegisterCommandHandler(repository, fakeDateTimeProvider)
                .Handle(command, default);
        }
    }


}
