using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rtl.TvMaze.Scraper.Repository;
using Rtl.TvMaze.Scraper.Service.Contracts;

namespace Rtl.TvMaze.Scraper.Scheduler
{
    public class ScraperHostInstant : BackgroundService
    {
        private readonly IServiceProvider _provider;
        public ScraperHostInstant(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DatabaseMigrate();
            await Scrape();
        }

        private async Task Scrape()
        {
            using (var scope = _provider.CreateScope())
            {
                var scraperServiceClient = scope.ServiceProvider.GetRequiredService<IScraperService>();
                await scraperServiceClient.ExecuteScraping();
            }
        }

        private async Task DatabaseMigrate()
        {
            using var scope = _provider.CreateScope();
            using var _dbContext = scope.ServiceProvider.GetRequiredService<TvMazeContext>();
            await _dbContext.Database.MigrateAsync();
            await _dbContext.Database.EnsureCreatedAsync();
        }
    }
}
