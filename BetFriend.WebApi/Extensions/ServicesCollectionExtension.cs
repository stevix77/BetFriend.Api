using Microsoft.Extensions.DependencyInjection;

namespace BetFriend.WebApi.Extensions
{
    internal static class ServicesCollectionExtension
    {
        internal static IServiceCollection AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(x =>
            {
                x.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            return services;
        }

        //internal static IServiceCollection AddAuthentificationExtension(this IServiceCollection services, string key)
        //{
        //    services.AddAuthentication()
        //    .AddJwtBearer(x =>
        //    {
        //        x.RequireHttpsMetadata = false;
        //        x.SaveToken = true;
        //        x.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        //            ValidateIssuer = false,
        //            ValidateAudience = false
        //        };
        //    });

        //    return services;
        //}
    }
}
