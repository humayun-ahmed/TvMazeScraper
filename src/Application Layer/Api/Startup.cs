// Copyright 2021, Rtl.

using System;
using System.IO;
using System.Net.Http;
using Infrastructure.Repository;
using Infrastructure.Repository.Contracts;
using Infrastructure.Scheduler;
using Infrastructure.Validator.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Rtl.TvMaze.Api.CustomMiddleware;
using Rtl.TvMaze.Api.Dto;
using Rtl.TvMaze.Api.HealthCheck;
using Rtl.TvMaze.Api.Logging;
using Rtl.TvMaze.Api.Options;
using Rtl.TvMaze.Scraper.Repository;
using Rtl.TvMaze.Scraper.Scheduler;
using Rtl.TvMaze.Scraper.Service;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts.Settings;
using Rtl.TvMaze.Validators;
using Serilog;

namespace Rtl.TvMaze.Api
{
    public class Startup
    {
        public Startup()
        {
            Configuration = GetConfiguration();
        }

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ".Development"}.json", optional: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            RegisterDependencies(services);

            services.AddHealthChecks()
                .AddCheck<HealthCheck.HealthCheck>("Tv maze service health check")
                .AddMemoryHealthCheck("memory");

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UserRequestLogger();

            //register first the global error handler!!!!
            app.ConfigureGlobalExceptionMiddleware();

            // Sequence of registration is important. Anything before this in not included in the logging.
            app.UseSerilogRequestLogging(options =>
                {
                    options.EnrichDiagnosticContext = LogDataEnricher.EnrichFromRequest;
                }
            );

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the tv maze service.");
                });
                endpoints.MapHealthChecks("/health"); //Add a health endpoint to return the status of your application.
                endpoints.MapControllers();
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {

            services.AddDependencies(Configuration);

            services.AddHostedService<ScraperHostInstant>();

            services.AddScheduler<ScraperHost>(c => c.Expression = Configuration.BindSettings<SchedulerSettings>(nameof(SchedulerSettings)).CronExpressionRecurrence);

            services.ConfigureSwagger();

            services.AddScoped<IValidator<ShowRequest>, ShowRequestValidator>();
        }
    }
}