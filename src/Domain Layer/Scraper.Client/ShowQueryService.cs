using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repository.Contracts;
using Infrastructure.Repository.Contracts.Filter;
using Microsoft.EntityFrameworkCore;
using Rtl.TvMaze.Scraper.Repository.Entity;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO;
using Rtl.TvMaze.Scraper.Service.Mappers;

namespace Rtl.TvMaze.Scraper.Service
{
    public class ShowQueryService : IShowQueryService
    {
        private readonly IReadOnlyRepository m_readOnlyRepository;
        
        public ShowQueryService(IReadOnlyRepository readOnlyRepository)
        {
            m_readOnlyRepository = readOnlyRepository;
        }

        public async Task<PagedResult<ShowDTO>> GetShows(ShowRequest filter)
        {
            var queryableResult = m_readOnlyRepository.GetItems<Show>().Include(i => i.Casts).GetPaged(filter.PageNo, filter.PageSize);

            PagedResult<ShowDTO> result = new PagedResult<ShowDTO>
            {
                PageCount = queryableResult.PageCount,
                CurrentPage = queryableResult.CurrentPage,
                PageSize = queryableResult.PageSize,
                RowCount = queryableResult.RowCount,
                Results = (await queryableResult.Results.Select(s => s.Map()).ToListAsync()).AsQueryable()
            };

            return result;
        }
    }
}
