using System;
using System.Net.Http;
using Infrastructure.Repository;
using Infrastructure.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Rtl.TvMaze.Scraper.Repository;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts.Settings;
using Rtl.TvMaze.Scraper.Service.Extensions;

namespace Rtl.TvMaze.Scraper.Service
{
    public static class DependencyBuilder
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region Settings
            services.AddSingleton(configuration.BindSettings<PaginationSettings>(nameof(PaginationSettings)));
            services.AddSingleton(configuration.BindSettings<ScraperSettings>(nameof(ScraperSettings)));
            services.AddSingleton(configuration.BindSettings<SchedulerSettings>(nameof(SchedulerSettings)));
            #endregion

            #region Http client
            services.AddHttpClient<IScraperService, ScraperService>(HttpClientConstants.USER_AGENT_HEADER).AddPolicyHandler(GetRetryPolicy());// behind the scene, DefaultHttpClientFactory is used
            #endregion

            #region DB
            services.AddDbContext<TvMazeContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(TvMazeContext))));
            #endregion

            #region Services
            services.AddScoped<IScraperService, ScraperService>();
            services.AddScoped<IShowQueryService, ShowQueryService>();
            #endregion

            #region ServiceClients

            #endregion

            #region Repositories
            services.AddScoped<BaseContext, TvMazeContext>();
            services.AddScoped<IRepository, Infrastructure.Repository.Repository>();
            services.AddScoped<IReadOnlyRepository, Infrastructure.Repository.Repository>();

            #endregion
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
