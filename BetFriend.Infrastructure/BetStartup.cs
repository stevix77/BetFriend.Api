namespace BetFriend.Bet.Infrastructure
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Usecases.LaunchBet;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.AzureStorage;
    using BetFriend.Bet.Infrastructure.DataAccess;
    using BetFriend.Bet.Infrastructure.Gateways;
    using BetFriend.Bet.Infrastructure.Repositories;
    using BetFriend.Shared.Application;
    using BetFriend.Shared.Application.Abstractions;
    using BetFriend.Shared.Domain;
    using BetFriend.Shared.Infrastructure;
    using BetFriend.Shared.Infrastructure.Configuration;
    using BetFriend.Shared.Infrastructure.Configuration.Behaviors;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using Serilog;
    using System;


    public static class BetStartup
    {
        public static IServiceCollection AddBetModule(this IServiceCollection services,
                                                             IConfiguration configuration)
        {
            var provider = Initialize(services, configuration);
            services.AddScoped<IBetModule>(x => new BetModule(provider));
            return services;
        }

        private static IServiceProvider Initialize(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton(x => configuration.GetSection("AzureStorage").Get<AzureStorageConfiguration>());
            serviceCollection.AddDbContext<DbContext, BetFriendContext>(options => options.UseSqlServer(configuration.GetConnectionString("BetFriendDbContext")));
            serviceCollection.AddScoped<IMemberRepository, MemberRepository>();
            serviceCollection.AddScoped(x =>
            {
                var mongoClient = new MongoClient(configuration.GetConnectionString("MongoServerUrl"));
                return mongoClient.GetDatabase(configuration["MongoDatabaseName"]);
            });
            serviceCollection.AddLogging();
            //serviceCollection.AddApplicationInsightsTelemetry(configuration["ApplicationInsightKey"]);
            serviceCollection.AddScoped<IAuthenticationGateway, AuthenticationGateway>();
            serviceCollection.AddScoped<IBetRepository, BetRepository>();
            serviceCollection.AddScoped<IBetQueryRepository, BetQueryRepository>();
            serviceCollection.AddScoped<IFeedRepository, FeedRepository>();
            serviceCollection.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            serviceCollection.AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
            serviceCollection.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            //serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            serviceCollection.AddMediatR(typeof(LaunchBetCommand).Assembly);
            return serviceCollection.BuildServiceProvider();
        }
    }
}
