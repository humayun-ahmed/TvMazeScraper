// Copyright 2021, Rtl.

using System;
using System.Net.Http;
using Infrastructure.Repository;
using Infrastructure.Repository.Contracts;
using Infrastructure.Scheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Rtl.TvMaze.Api.CustomMiddleware;
using Rtl.TvMaze.Api.HealthCheck;
using Rtl.TvMaze.Api.Logging;
using Rtl.TvMaze.Api.Options;
using Rtl.TvMaze.Scraper.Repository;
using Rtl.TvMaze.Scraper.Scheduler;
using Rtl.TvMaze.Scraper.Service;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts.Settings;
using Serilog;

namespace Rtl.TvMaze.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings{(env.IsDevelopment() ? ".Development" : string.Empty)}.json", false, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
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

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BaseContext>();
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            #region Settings
            services.AddSingleton(Configuration.BindSettings<PaginationSettings>(nameof(PaginationSettings)));
            services.AddSingleton(Configuration.BindSettings<ScraperSettings>(nameof(ScraperSettings)));
            services.AddSingleton(Configuration.BindSettings<RateLimitSettings>(nameof(RateLimitSettings)));
            services.AddSingleton(Configuration.BindSettings<SchedulerSettings>(nameof(SchedulerSettings)));
            #endregion

            #region Http client
            services.AddHttpClient<IScraperService, ScraperService>(HttpClientConstants.USER_AGENT_HEADER).AddPolicyHandler(GetRetryPolicy());// behind the scene, DefaultHttpClientFactory is used
            #endregion

            #region DB
            services.AddDbContext<TvMazeContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(TvMazeContext))));
            #endregion

            #region Services
            services.AddScoped<IScraperService, ScraperService>();
            services.AddScoped<IShowQueryService, ShowQueryService>();
            #endregion

            #region ServiceClients

            #endregion

            #region Repositories
            services.AddScoped<BaseContext, TvMazeContext>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IReadOnlyRepository, Repository>();

            #endregion

            #region Hosted services
            services.AddHostedService<ScraperHostInstant>();
            #endregion

            #region Schedulers
            services.AddScheduler<ScraperHost>(c => c.Expression = Configuration.BindSettings<SchedulerSettings>(nameof(SchedulerSettings)).CronExpressionRecurrence);
            #endregion

            services.ConfigureSwagger();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}