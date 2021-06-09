using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            await Scrape();
        }

       

        #region Seed
        private async Task Scrape()
        {
            using (var scope = _provider.CreateScope())
            {
                var scraperServiceClient = scope.ServiceProvider.GetRequiredService<IScraperService>();
                await scraperServiceClient.ExecuteScraping();
            }
        }
        #endregion
    }
}
