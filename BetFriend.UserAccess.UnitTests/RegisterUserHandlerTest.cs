using System;
using System.Collections;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UserAccess.UnitTests
{
    public class RegisterUserHandlerTest
    {
        [Fact]
        public async Task RegisterUser()
        {
            IUserRepository repository = new InMemoryUserRepository();
            IDateTimeProvider fakeDateTimeProvider = new FakeDateTimeProvider();
            var handler = new RegisterCommandHandler(repository, fakeDateTimeProvider);
            await handler.Handle(new RegisterCommand(Guid.NewGuid(), "username", "password", "email@email.com"), default);
            Assert.Single(repository.GetUsers());
        }
    }

    internal interface IDateTimeProvider
    {
    }

    internal class FakeDateTimeProvider : IDateTimeProvider
    {
        public FakeDateTimeProvider()
        {
        }
    }

    internal interface IUserRepository
    {
        IEnumerable GetUsers();
    }

    internal class RegisterCommandHandler
    {
        private object repository;
        private object fakeDateTimeProvider;

        public RegisterCommandHandler(object repository, object fakeDateTimeProvider)
        {
            this.repository = repository;
            this.fakeDateTimeProvider = fakeDateTimeProvider;
        }

        internal Task Handle(RegisterCommand registerCommand, object p)
        {
            throw new NotImplementedException();
        }
    }

    internal class RegisterCommand
    {
        private Guid guid;
        private string v1;
        private string v2;
        private string v3;

        public RegisterCommand(Guid guid, string v1, string v2, string v3)
        {
            this.guid = guid;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    internal class InMemoryUserRepository : IUserRepository
    {
        public InMemoryUserRepository()
        {
        }

        internal IEnumerable GetUsers()
        {
            throw new NotImplementedException();
        }

        IEnumerable IUserRepository.GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
