using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BetFriend.AzureFunctions.Startup))]
namespace BetFriend.AzureFunctions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Usecases.LaunchBet;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure;
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
    using BetFriend.UserAccess.Infrastructure;
    using MediatR;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseAppSettings();
            builder.Services.AddSingleton(x => builder.GetContext().Configuration.GetSection("AzureStorage").Get<AzureStorageConfiguration>());
            builder.Services.AddDbContext<DbContext, BetFriendContext>(options => options.UseSqlServer(builder.GetContext().Configuration.GetConnectionString("BetFriendDbContext")));
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped(x =>
            {
                var mongoClient = new MongoClient(builder.GetContext().Configuration.GetConnectionString("MongoServerUrl"));
                return mongoClient.GetDatabase(builder.GetContext().Configuration["MongoDatabaseName"]);
            });
            builder.Services.AddLogging();
            builder.Services.AddScoped<IAuthenticationGateway, AuthenticationGateway>();
            builder.Services.AddScoped<IBetRepository, BetRepository>();
            builder.Services.AddScoped<IBetQueryRepository, BetQueryRepository>();
            builder.Services.AddScoped<IFeedRepository, FeedRepository>();
            builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            builder.Services.AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
            builder.Services.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            builder.Services.AddMediatR(typeof(LaunchBetCommand).Assembly);
            builder.Services.AddScoped<IBetModule, BetModule>();
        }
    }
}
