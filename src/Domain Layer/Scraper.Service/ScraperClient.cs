using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO.Base;
using Rtl.TvMaze.Scraper.Service.Contracts.Model;
using Rtl.TvMaze.Scraper.Service.Contracts.Model.Cast;
using Rtl.TvMaze.Scraper.Service.Contracts.Settings;
using Rtl.TvMaze.Scraper.Service.Extensions;

namespace Rtl.TvMaze.Scraper.Service
{
    public class ScraperClient: IScraperClient
    {
        private readonly ScraperSettings m_scraperSettings;
        private readonly HttpClient m_httpClient;

        public ScraperClient(IHttpClientFactory httpClientFactory, ScraperSettings scraperSettings)
        {
            m_scraperSettings = scraperSettings;
            m_httpClient = httpClientFactory.CreateClient(HttpClientConstants.USER_AGENT_UNIQUE_ID);
            m_httpClient.DefaultRequestHeaders.Add(HttpClientConstants.USER_AGENT_HEADER, HttpClientConstants.USER_AGENT_UNIQUE_ID); 
        }
        public async Task<BaseDTO<List<ShowModel>>> GetShows(int page)
        {
            var result = new BaseDTO<List<ShowModel>>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{m_scraperSettings.ApiBaseUrl}/{m_scraperSettings.ShowsRoute.Replace("{page}", page.ToString())}");
            var response = await m_httpClient.SendAsync(request);

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

        public async Task<BaseDTO<List<CastModel>>> GetCast(int showId)
        {
            var result = new BaseDTO<List<CastModel>>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{m_scraperSettings.ApiBaseUrl}/{m_scraperSettings.CastRoute.Replace("{id}", showId.ToString())}");
            var response = await m_httpClient.SendAsync(request);

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
