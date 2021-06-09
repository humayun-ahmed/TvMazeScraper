using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Repository.Contracts;
using Microsoft.Extensions.Logging;
using Rtl.TvMaze.Scraper.Repository.Entity;
using Rtl.TvMaze.Scraper.Service.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO.Base;
using Rtl.TvMaze.Scraper.Service.Contracts.Model;
using Rtl.TvMaze.Scraper.Service.Contracts.Model.Cast;
using Rtl.TvMaze.Scraper.Service.Contracts.Settings;
using Rtl.TvMaze.Scraper.Service.Extensions;

namespace Rtl.TvMaze.Scraper.Service
{
    public class ScraperService : IScraperService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ScraperSettings _scraperSettings;
        private readonly ILogger<ScraperService> _logger;
        private readonly IRepository m_repository;

        public ScraperService(IHttpClientFactory httpClientFactory, ScraperSettings scraperSettings,
            ILogger<ScraperService> logger, IRepository repository)
        {
            _httpClientFactory = httpClientFactory;
            _scraperSettings = scraperSettings;
            _logger = logger;
            m_repository = repository;
        }

        public async Task ExecuteScraping()
        {
            var client = _httpClientFactory.CreateClient(HttpClientConstants.USER_AGENT_UNIQUE_ID);
            client.DefaultRequestHeaders.Add(HttpClientConstants.USER_AGENT_HEADER, HttpClientConstants.USER_AGENT_UNIQUE_ID);

            var page = 0;
            var dbShows = await m_repository.CountAsync<Show>();
            
            if (dbShows > 0)
                page = (int)Math.Floor((double)dbShows / _scraperSettings.PageSize);

            while (true)
            {
                var getShowsResult = await GetShows(page, client);
                if (!getShowsResult.Success)
                {
                    _logger.LogCritical(getShowsResult.ErrorMessage);
                    return;
                }

                var shows = getShowsResult.Data;
                if (shows == null || !shows.Any())
                {
                    _logger.LogInformation("Scraping finished successfully.");
                    return;
                }

                foreach (var show in shows)
                {
                    var showEntity = await AddShow(show);

                    var getCastsResult = await GetCast(show.Id, client);
                    if (!getCastsResult.Success)
                    {
                        _logger.LogCritical(getCastsResult.ErrorMessage);
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

        private async Task<BaseDTO<List<ShowModel>>> GetShows(int page, HttpClient client)
        {
            var result = new BaseDTO<List<ShowModel>>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_scraperSettings.ApiBaseUrl}/{_scraperSettings.ShowsRoute.Replace("{page}", page.ToString())}");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                result.AddError(await response.Content.ReadAsStringAsync());
                return result;
            }

            var content = await response.Content.ReadAsStringAsync();
            var deserializeResult = content.TryDeserialize<List<ShowModel>>();
            if (!deserializeResult.Success)
            {
                result.AddError(deserializeResult.ErrorMessage);
                return result;
            }

            result.Data = deserializeResult.Data;

            return result;
        }

        private async Task<BaseDTO<List<CastModel>>> GetCast(int showId, HttpClient client)
        {
            var result = new BaseDTO<List<CastModel>>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_scraperSettings.ApiBaseUrl}/{_scraperSettings.CastRoute.Replace("{id}", showId.ToString())}");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                result.AddError(await response.Content.ReadAsStringAsync());
                return result;
            }

            var content = await response.Content.ReadAsStringAsync();
            var deserializeResult = content.TryDeserialize<List<CastModel>>();
            if (!deserializeResult.Success)
            {
                result.AddError(deserializeResult.ErrorMessage);
                return result;
            }

            result.Data = deserializeResult.Data;

            return result;
        }
    }
}
