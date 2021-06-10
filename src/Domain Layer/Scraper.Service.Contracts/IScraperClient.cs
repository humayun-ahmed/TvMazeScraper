using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO.Base;
using Rtl.TvMaze.Scraper.Service.Contracts.Model;
using Rtl.TvMaze.Scraper.Service.Contracts.Model.Cast;

namespace Rtl.TvMaze.Scraper.Service.Contracts
{
    public interface IScraperClient
    {
        Task<BaseDTO<List<ShowModel>>> GetShows(int page);
        Task<BaseDTO<List<CastModel>>> GetCast(int showId);
    }
}
