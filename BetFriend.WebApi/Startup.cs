namespace BetFriend.WebApi
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Usecases.LaunchBet;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Feeds;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure;
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
    using BetFriend.Shared.Infrastructure.DateTimeProvider;
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Application.Usecases.Register;
    using BetFriend.UserAccess.Application.Usecases.SignIn;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using BetFriend.UserAccess.Infrastructure;
    using BetFriend.UserAccess.Infrastructure.DataAccess;
    using BetFriend.UserAccess.Infrastructure.Hash;
    using BetFriend.UserAccess.Infrastructure.Repositories;
    using BetFriend.UserAccess.Infrastructure.TokenGenerator;
    using BetFriend.WebApi.Controllers.Register;
    using BetFriend.WebApi.Controllers.SignIn;
    using BetFriend.WebApi.Extensions;
    using BetFriend.WebApi.Filters;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsExtension();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.IgnoreNullValues = true;
                o.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BetFriend.WebApi", Version = "v1" });
            });
            
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IBetProcessor, BetProcessor>();
            services.AddUserAccessModule(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            BetStartup.Initialize(Configuration);
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetFriend.WebApi v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
