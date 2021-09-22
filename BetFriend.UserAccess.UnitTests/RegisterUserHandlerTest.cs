namespace BetFriend.UserAccess.UnitTests
{
    using BetFriend.Shared.Infrastructure.DateTimeProvider;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.Hash;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class RegisterUserHandlerTest
    {
        private readonly InMemoryUserRepository userRepository;
        private readonly FakeDateTimeProvider fakeDateTimeProvider;

        public RegisterUserHandlerTest()
        {
            userRepository = new InMemoryUserRepository();
            fakeDateTimeProvider = new FakeDateTimeProvider(new DateTime(2021, 9, 22));
        }
        [Fact]
        public async Task ShouldRegisterAUser()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            await RegisterUser(command, hashPassword: new MD5HashPassword());
            AssertThatUserIsRegistered();
        }

        [Fact]
        public async Task ShouldNotRegisterIfUsernameAlreadyExist()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            var repo = new InMemoryUserRepository(new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 22)));
            await RegisterUser(command, repo, new MD5HashPassword());
            AssertThatContainsOneUser(repo);
        }

        [Fact]
        public async Task ShouldNotRegisterIfEmailAlreadyExist()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            var repo = new InMemoryUserRepository(new User("abc", "username2", "email@email.com", "password", new DateTime(2021, 9, 22)));
            await RegisterUser(command, repo, new MD5HashPassword());
            AssertThatContainsOneUser(repo);
        }

        [Fact]
        public async Task ShouldNotRegisterUserIfUserIdIsEmpty()
        {
            var command = new RegisterCommand("", "username", "password", "email@email.com");
            var record = await Record.ExceptionAsync(() => RegisterUser(command, userRepository, hashPassword: new MD5HashPassword()));
            Assert.IsType<UserIdNotValidException>(record);
        }

        [Fact]
        public async Task ShouldNotRegisterUserIfUserIdIsNull()
        {
            var command = new RegisterCommand(null, "username", "password", "email@email.com");
            var record = await Record.ExceptionAsync(() => RegisterUser(command, userRepository, hashPassword: new MD5HashPassword()));
            Assert.IsType<UserIdNotValidException>(record);
        }

        private static void AssertThatContainsOneUser(InMemoryUserRepository repo)
        {
            Assert.Single(repo.GetUsers());
        }

        private void AssertThatUserIsRegistered()
        {
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 22));
            Assert.Equal(user, userRepository.GetUsers().Single());
        }

        private async Task RegisterUser(RegisterCommand command, InMemoryUserRepository inMemoryUserRepository = null, MD5HashPassword hashPassword = null)
        {
            await new RegisterCommandHandler(inMemoryUserRepository ?? userRepository,
                                            fakeDateTimeProvider,
                                            hashPassword)
                .Handle(command, default);
        }
    }
}
