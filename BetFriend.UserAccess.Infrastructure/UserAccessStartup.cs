namespace BetFriend.UserAccess.Infrastructure
{
    using BetFriend.Shared.Application;
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.Shared.Domain;
    using BetFriend.Shared.Infrastructure;
    using BetFriend.Shared.Infrastructure.Configuration;
    using BetFriend.Shared.Infrastructure.Configuration.Behaviors;
    using BetFriend.Shared.Infrastructure.DateTimeProvider;
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure.AzureStorage;
    using BetFriend.UserAccess.Infrastructure.Configurations;
    using BetFriend.UserAccess.Infrastructure.DataAccess;
    using BetFriend.UserAccess.Infrastructure.Hash;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using BetFriend.UserAccess.Infrastructure.TokenGenerator;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public static class UserAccessStartup
    {
        public static void AddUserAccessModule(IConfiguration configuration, ILogger logger)
        {
            Initialize(configuration, logger);
        }

        private static void Initialize(IConfiguration configuration, ILogger logger)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(x => x.AddSerilog(logger));
            serviceCollection.AddSingleton(x => configuration.GetSection("AzureStorage").Get<AzureStorageConfiguration>());
            serviceCollection.AddSingleton(x => configuration.GetSection("Authentification").Get<AuthenticationConfiguration>());
            serviceCollection.AddTransient<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddDbContext<DbContext, UserAccessContext>(options => options.UseSqlServer(configuration.GetConnectionString("UserAccessDbContext")));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            serviceCollection.AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
            serviceCollection.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            serviceCollection.AddScoped<IHashPassword, Sha256HashPassword>();
            serviceCollection.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            serviceCollection.AddMediatR(typeof(RegisterCommand).Assembly);
            UserAccessCompositionRoot.SetProvider(serviceCollection.BuildServiceProvider());
        }
    }
}
