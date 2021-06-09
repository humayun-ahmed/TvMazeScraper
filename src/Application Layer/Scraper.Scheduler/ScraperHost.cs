using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Scheduler;
using Infrastructure.Scheduler.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rtl.TvMaze.Scraper.Service.Contracts;

namespace Rtl.TvMaze.Scraper.Scheduler
{
    public class ScraperHost : SchedulerHostedService
    {
        private readonly ILogger<ScraperHost> _logger;
        private readonly IServiceProvider _provider;
        public ScraperHost(IScheduleConfig<ScraperHost> config, ILogger<ScraperHost> logger,
            IServiceProvider provider)
            : base(config.Expression)
        {
            _logger = logger;
            _provider = provider;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = _provider.CreateScope())
                {
                    var scraperServiceClient = scope.ServiceProvider.GetRequiredService<IScraperService>();
                    await scraperServiceClient.ExecuteScraping();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
            }
        }
    }
}
