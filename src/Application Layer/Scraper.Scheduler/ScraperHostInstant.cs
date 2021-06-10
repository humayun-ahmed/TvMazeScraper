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
        private readonly IServiceProvider m_provider;
        public ScraperHostInstant(IServiceProvider provider)
        {
            m_provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DatabaseMigrate();
            await Scrape();
        }

        private async Task Scrape()
        {
            using (var scope = m_provider.CreateScope())
            {
                var scraperServiceClient = scope.ServiceProvider.GetRequiredService<IScraperService>();
                await scraperServiceClient.ExecuteScraping();
            }
        }

        private async Task DatabaseMigrate()
        {
            using var scope = m_provider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<TvMazeContext>();
            await dbContext.Database.MigrateAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}
