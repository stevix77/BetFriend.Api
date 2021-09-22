using BetFriend.Bet.Application.Abstractions;
using BetFriend.Bet.Application.Abstractions.Repository;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Feeds;
using BetFriend.Bet.Domain.Members;
using BetFriend.Bet.Infrastructure;
using BetFriend.Bet.Infrastructure.AzureStorage;
using BetFriend.Bet.Infrastructure.DataAccess;
using BetFriend.Bet.Infrastructure.Repositories;
using BetFriend.Bet.Infrastructure.Repositories.InMemory;
using BetFriend.Shared.Application;
using BetFriend.Shared.Application.Abstractions;
using BetFriend.Shared.Domain;
using BetFriend.Shared.Infrastructure;
using BetFriend.Shared.Infrastructure.Configuration;
using BetFriend.Shared.Infrastructure.Configuration.Behaviors;
using BetFriend.Shared.Infrastructure.DateTimeProvider;
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

namespace BetFriend.WebApi
{
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

            services.AddDbContext<DbContext, BetFriendContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BetFriendDbContext")));
            services.AddLogging();
            services.AddScoped(x =>
            {
                var mongoClient = new MongoClient();
                return mongoClient.GetDatabase(Configuration[""]);
            });
            services.AddSingleton<AzureStorageConfiguration>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IProcessor, Processor>();
            services.AddScoped<IBetRepository, BetRepository>();
            services.AddScoped<IBetQueryRepository>(x => new InMemoryBetQueryRepository());
            services.AddScoped<IFeedRepository>(x => new InMemoryFeedRepository());
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddScoped<IDomainEventsListener, DomainEventsListener>();
            services.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            services.AddScoped<IMemberRepository>(x => new InMemoryMemberRepository(new List<Member>()
            {
                new Member(new MemberId(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1")),
                            "memberName",
                            100) }));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            services.AddMediatR(typeof(Bet.Application.Usecases.LaunchBet.LaunchBetCommand).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetFriend.WebApi v1"));
            }

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
