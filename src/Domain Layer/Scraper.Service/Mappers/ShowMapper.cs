using System.Linq;
using Rtl.TvMaze.Scraper.Repository.Entity;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO;

namespace Rtl.TvMaze.Scraper.Service.Mappers
{
    public static class ShowMapper
    {
        public static ShowDTO Map(this Show show)
        {
            return show != null
                ?
                new ShowDTO
                {
                    Id = show.Id,
                    Name = show.Name,
                    Cast = show.Casts.OrderByDescending(d => d.Birthday).Select(s => s.Map()).ToList()
                }
                : 
                null;
        }
    }
}
