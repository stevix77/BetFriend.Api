using BetFriend.Bet.Application.Abstractions.Repository;
using BetFriend.Bet.Application.Usecases.LaunchBet;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Feeds;
using BetFriend.Bet.Domain.Members;
using BetFriend.Bet.Infrastructure.AzureStorage;
using BetFriend.Bet.Infrastructure.DataAccess;
using BetFriend.Bet.Infrastructure.Gateways;
using BetFriend.Bet.Infrastructure.Repositories.InMemory;
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
using System;
using System.Collections.Generic;

namespace BetFriend.Bet.Infrastructure
{
    public static class BetStartup
    {
        private static readonly Guid _memberId = Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1");

        public static void Initialize(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<AzureStorageConfiguration>();
            serviceCollection.AddDbContext<DbContext, BetFriendContext>(options => options.UseSqlServer(configuration.GetConnectionString("BetFriendDbContext")));
            serviceCollection.AddScoped<IMemberRepository>(x => new InMemoryMemberRepository(new List<Member>()
            {
                new Member(new MemberId(_memberId),
                            "memberName",
                            100) }));
            serviceCollection.AddScoped(x =>
            {
                var mongoClient = new MongoClient();
                return mongoClient.GetDatabase("");
            });
            serviceCollection.AddLogging();
            serviceCollection.AddScoped<IAuthenticationGateway>(x => new InMemoryAuthenticationGateway(_memberId));
            serviceCollection.AddScoped<IBetRepository, InMemoryBetRepository>();
            serviceCollection.AddScoped<IBetQueryRepository>(x => new InMemoryBetQueryRepository());
            serviceCollection.AddScoped<IFeedRepository>(x => new InMemoryFeedRepository());
            serviceCollection.AddScoped<IFeedQueryRepository>(x => new InMemoryFeedQueryRepository());
            serviceCollection.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            serviceCollection.AddScoped<IDomainEventsListener, DomainEventsListener>();
            serviceCollection.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            serviceCollection.AddMediatR(typeof(LaunchBetCommand).Assembly);
            var _serviceProvider = serviceCollection.BuildServiceProvider();
            BetCompositionRoot.SetProvider(_serviceProvider);
        }
    }
}
