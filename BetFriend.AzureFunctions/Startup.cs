using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BetFriend.AzureFunctions.Startup))]
namespace BetFriend.AzureFunctions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Infrastructure;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;
            builder.UseAppSettings();
            builder.Services.AddScoped<IBetModule, BetModule>();
            BetStartup.AddBetModule(configuration, default, default);
        }
    }
}
