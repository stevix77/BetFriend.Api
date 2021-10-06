namespace BetFriend.UserAccess.Infrastructure
{
    using BetFriend.Shared.Application;
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.Shared.Domain;
    using BetFriend.Shared.Infrastructure;
    using BetFriend.Shared.Infrastructure.Configuration;
    using BetFriend.Shared.Infrastructure.Configuration.Behaviors;
    using BetFriend.Shared.Infrastructure.DateTimeProvider;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using BetFriend.UserAccess.Application.Usecases.SignIn;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.AzureStorage;
    using BetFriend.UserAccess.Infrastructure.DataAccess;
    using BetFriend.UserAccess.Infrastructure.Gateways;
    using BetFriend.UserAccess.Infrastructure.Hash;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using BetFriend.UserAccess.Infrastructure.TokenGenerator;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class UserAccessStartup
    {
        private static readonly Guid _memberId = Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1");

        public static void Initialize(IConfiguration configuration,
                                      IRegisterPresenter registerPresenter,
                                      ISignInPresenter signInPresenter)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<AzureStorageConfiguration>();
            serviceCollection.AddTransient<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddDbContext<DbContext, UserAccessContext>(options => options.UseSqlServer(configuration.GetConnectionString("UserAccessDbContext")));
            serviceCollection.AddScoped<IAuthenticationGateway>(x => new InMemoryAuthenticationGateway(_memberId));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            serviceCollection.AddScoped<IDomainEventsListener, DomainEventsListener>();
            serviceCollection.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            serviceCollection.AddScoped<IHashPassword, Sha256HashPassword>();
            serviceCollection.AddScoped<ITokenGenerator>(x => new InMemoryTokenGenerator("jwtToken"));
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped(x => registerPresenter);
            serviceCollection.AddScoped(x => signInPresenter);

            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            serviceCollection.AddMediatR(typeof(RegisterCommand).Assembly);
            var _serviceProvider = serviceCollection.BuildServiceProvider();
            UserAccessCompositionRoot.SetProvider(_serviceProvider);
        }
    }
}
