using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BetFriend.AzureFunctions.Startup))]
namespace BetFriend.AzureFunctions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Infrastructure;
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Infrastructure;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var configurationRoot = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            ////builder.AddDependencies();
            builder.Services.AddBetModule(default);
            //builder.Services.AddScoped<IUserAccessModule, UserAccessModule>();
        }
    }
}
