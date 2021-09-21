﻿using BetFriend.UserAccess.Application.Usecases.Register;
using BetFriend.UserAccess.Domain;
using BetFriend.UserAccess.Domain.Users;
using BetFriend.UserAccess.Infrastructure;
using BetFriend.UserAccess.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BetFriend.UserAccess.UnitTests
{
    public class RegisterUserHandlerTest
    {
        private readonly InMemoryUserRepository repository;
        private readonly FakeDateTimeProvider fakeDateTimeProvider;
        private readonly User _user;

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
