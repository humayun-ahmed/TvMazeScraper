using System.Threading.Tasks;
using Infrastructure.Repository.Contracts.Filter;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO;

namespace Rtl.TvMaze.Scraper.Service.Contracts
{
    public interface IShowQueryService
    {
        Task<PagedResult<ShowDTO>> GetShows(ShowRequest show);
    }
}
