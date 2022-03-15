namespace BetFriend.WebApi
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Infrastructure;
    using BetFriend.UserAccess.Application.Abstractions;
    using BetFriend.UserAccess.Infrastructure;
    using BetFriend.WebApi.Extensions;
    using BetFriend.WebApi.Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using System;

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
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BetFriend.WebApi", Version = "v1" });
            });

            services.AddLogging();
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsightKey"]);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAccessModule, UserAccessModule>();
            services.AddScoped<IBetModule, BetModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.ApplicationInsights(Configuration["ApplicationInsightKey"],
                                            TelemetryConverter.Traces)
                .CreateLogger();

            UserAccessStartup.AddUserAccessModule(Configuration, logger);
            BetStartup.AddBetModule(Configuration, httpContextAccessor, logger);

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetFriend.WebApi v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
