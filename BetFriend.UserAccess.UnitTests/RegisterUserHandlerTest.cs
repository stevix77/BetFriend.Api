namespace BetFriend.UserAccess.UnitTests
{
    using BetFriend.Shared.Application;
    using BetFriend.Shared.Infrastructure.DateTimeProvider;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Domain.Users.Events;
    using BetFriend.UserAccess.Infrastructure.Hash;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using BetFriend.UserAccess.Infrastructure.TokenGenerator;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class RegisterUserHandlerTest
    {
        private const string JWT_TOKEN = "jwtToken";
        private readonly InMemoryUserRepository userRepository;
        private readonly FakeDateTimeProvider fakeDateTimeProvider;
        private readonly DomainEventsAccessor domainEventsListener;
        private readonly InMemoryTokenGenerator inMemoryTokenGenerator;

        public RegisterUserHandlerTest()
        {
            domainEventsListener = new DomainEventsAccessor();
            userRepository = new InMemoryUserRepository(domainEventsAccessor: domainEventsListener);
            fakeDateTimeProvider = new FakeDateTimeProvider(new DateTime(2021, 9, 22));
            inMemoryTokenGenerator = new InMemoryTokenGenerator(JWT_TOKEN);
        }
        [Fact]
        public async Task ShouldRegisterAUser()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            var token = await RegisterUser(command, hashPassword: new MD5HashPassword());
            AssertThatUserIsRegistered(JWT_TOKEN, token);
        }

        [Fact]
        public async Task ShouldNotRegisterIfUsernameAlreadyExist()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            var repo = new InMemoryUserRepository(new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 22)));
            var record = await Record.ExceptionAsync(() => RegisterUser(command, repo, new MD5HashPassword()));
            AssertThatThrowUsernameAlreadyExistsException(record);
        }

        private static void AssertThatThrowUsernameAlreadyExistsException(Exception record)
        {
            Assert.IsType<UsernameAlreadyExistsException>(record);
        }

        [Fact]
        public async Task ShouldNotRegisterIfEmailAlreadyExist()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email.com");
            var repo = new InMemoryUserRepository(new User("abc", "username2", "email@email.com", "password", new DateTime(2021, 9, 22)));
            var record = await Record.ExceptionAsync(() => RegisterUser(command, repo, new MD5HashPassword()));
            AssertThatThrowEmailAlreadyExistsException(record);
        }

        private static void AssertThatThrowEmailAlreadyExistsException(Exception record)
        {
            Assert.IsType<EmailAlreadyExistsException>(record);
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

        [Fact]
        public async Task ShouldNotRegisterUserIfEmailIsNotValid()
        {
            var command = new RegisterCommand("abc", "username", "password", "email@email");
            var record = await Record.ExceptionAsync(() => RegisterUser(command, userRepository, hashPassword: new MD5HashPassword()));
            Assert.IsType<EmailNotValidException>(record);
        }

        private void AssertThatUserIsRegistered(string token, string actualToken)
        {
            var user = new User("abc", "username", "email@email.com", "password", new DateTime(2021, 9, 22));
            Assert.Equal(user, userRepository.GetUsers().Single());
            var domainEvent = domainEventsListener.GetDomainEvents().SingleOrDefault();
            Assert.Equal(new UserRegistered("abc", "email@email.com"), domainEvent);
            Assert.Equal(token, actualToken);
        }

        private async Task<string> RegisterUser(RegisterCommand command,
                                        InMemoryUserRepository inMemoryUserRepository = null,
                                        MD5HashPassword hashPassword = null)
        {
            return await new RegisterCommandHandler(inMemoryUserRepository ?? userRepository,
                                            fakeDateTimeProvider,
                                            hashPassword,
                                            inMemoryTokenGenerator)
                .Handle(command, default);
        }
    }
}
