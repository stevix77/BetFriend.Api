using BetFriend.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using BetFriend.WebApi.Extensions;
using BetFriend.WebApi.Filters;
using BetFriend.Infrastructure;
using BetFriend.Application.Abstractions;
using MediatR;
using BetFriend.Infrastructure.Repositories.InMemory;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Members;
using System.Collections.Generic;
using System;
using BetFriend.Infrastructure.Configuration.Behaviors;
using BetFriend.Application;
using BetFriend.Domain;
using BetFriend.Infrastructure.AzureStorage;

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
                o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BetFriend.WebApi", Version = "v1" });
            });

            services.AddDbContext<DbContext, BetFriendContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BetFriendDbContext")));
            services.AddLogging();
            services.AddScoped<IProcessor, Processor>();
            services.AddScoped<IBetRepository, InMemoryBetRepository>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddScoped<IDomainEventsListener, DomainEventsListener>();
            services.AddScoped<IStorageDomainEventsRepository, AzureStorageDomainEventsRepository>();
            services.AddScoped<IMemberRepository>(x => new InMemoryMemberRepository(new List<Member>() { new Member(new MemberId(Guid.Parse("01c1da98-b4b7-45dc-8352-c98ece06dab1")), 100) }));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            services.AddMediatR(typeof(Application.Usecases.LaunchBet.LaunchBetCommand).Assembly);
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
