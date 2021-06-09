using System.Collections.Generic;

namespace Rtl.TvMaze.Scraper.Service.Contracts.DTO
{
    public class ShowDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CastDTO> Cast { get; set; }
    }
}
