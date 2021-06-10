using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repository.Contracts;
using Microsoft.Extensions.Logging;
using Rtl.TvMaze.Scraper.Repository.Entity;
using Rtl.TvMaze.Scraper.Service.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.Model;
using Rtl.TvMaze.Scraper.Service.Contracts.Model.Cast;
using Rtl.TvMaze.Scraper.Service.Contracts.Settings;

namespace Rtl.TvMaze.Scraper.Service
{
    public class ScraperService : IScraperService
    {
        private readonly ScraperSettings m_scraperSettings;

        private readonly IScraperClient m_scraperClient;

        private readonly ILogger<ScraperService> m_logger;

        private readonly IRepository m_repository;

        public ScraperService(ScraperSettings scraperSettings, IScraperClient scraperClient,
            ILogger<ScraperService> logger, IRepository repository)
        {
            m_scraperSettings = scraperSettings;
            m_scraperClient = scraperClient;
            m_logger = logger;
            m_repository = repository;
        }

        public async Task ExecuteScraping()
        {
            var page = 0;
            var dbShows = await m_repository.CountAsync<Show>();
            
            if (dbShows > 0)
                page = (int)Math.Floor((double)dbShows / m_scraperSettings.PageSize);

            while (true)
            {
                var getShowsResult = await m_scraperClient.GetShows(page);
                if (!getShowsResult.Success)
                {
                    m_logger.LogCritical(getShowsResult.ErrorMessage);
                    return;
                }

                var shows = getShowsResult.Data;
                if (shows == null || !shows.Any())
                {
                    m_logger.LogInformation("Scraping finished successfully.");
                    return;
                }

                foreach (var show in shows)
                {
                    var showEntity = await AddShow(show);

                    var getCastsResult = await m_scraperClient.GetCast(show.Id);
                    if (!getCastsResult.Success)
                    {
                        m_logger.LogCritical(getCastsResult.ErrorMessage);
                        return;
                    }

                    var casts = getCastsResult.Data;
                    foreach (var cast in casts)
                        await AddCast(showEntity.Id, cast);
                }

                page++;
            }
        }

        private async Task AddCast(int showId, CastModel cast)
        {
            var birthdayParse = DateTime.TryParseExact(cast.Person.Birthday, FormatConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthday)
                ? birthday : (DateTime?)null;

            var castEntity = new Cast(cast.Person.Name, birthdayParse, showId);

            if (!await m_repository.AnyAsync<Cast>(a => a.ShowId == showId && a.Name == cast.Person.Name))
            {
                m_repository.Add<Cast>(castEntity);
                await m_repository.SaveChanges();
            }
        }

        private async Task<Show> AddShow(ShowModel show)
        {
            var showEntity = new Show(show.Name);

            if (!await m_repository.AnyAsync<Show>(a => a.Name == show.Name))
            {
                m_repository.Add<Show>(showEntity);
                await m_repository.SaveChanges();
            }
            else
                showEntity = await m_repository.GetItem<Show>(g => g.Name == show.Name);

            return showEntity;
        }
    }
}
