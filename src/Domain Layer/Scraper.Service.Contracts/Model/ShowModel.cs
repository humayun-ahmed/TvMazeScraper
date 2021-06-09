using Newtonsoft.Json;

namespace Rtl.TvMaze.Scraper.Service.Contracts.Model
{
    public class ShowModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
