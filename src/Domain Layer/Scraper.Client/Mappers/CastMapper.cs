using Rtl.TvMaze.Scraper.Repository.Entity;
using Rtl.TvMaze.Scraper.Service.Constants;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO;

namespace Rtl.TvMaze.Scraper.Service.Mappers
{
    public static class CastMapper
    {
        public static CastDTO Map(this Cast cast)
        {
            return cast != null
                ?
                new CastDTO
                {
                    Id = cast.Id,
                    Name = cast.Name,
                    Birthday = cast.Birthday?.ToString(FormatConstants.DateFormat).Replace("/", "-")
                }
                :
                null;
        }
    }
}
