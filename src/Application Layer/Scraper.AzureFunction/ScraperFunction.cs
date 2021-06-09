using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Rtl.TvMaze.Scraper.Service.Contracts;

namespace Rtl.TvMaze.Scraper.AzureFunction
{
    public class ScraperFunction
    {
        private readonly IScraperService m_scraperService;

        public ScraperFunction(IScraperService scraperService)
        {
            m_scraperService = scraperService;
        }

        [FunctionName("ScraperFunction")]
        public async Task Run([TimerTrigger("%TimerTriggerExpression%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"ScraperFunction, due to start at {myTimer.ScheduleStatus.Next}, has been started at {DateTime.UtcNow}.");

            await m_scraperService.ExecuteScraping();

            log.LogInformation($"ScraperFunction started.");
        }
    }
}
