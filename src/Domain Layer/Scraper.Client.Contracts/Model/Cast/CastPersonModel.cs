using Newtonsoft.Json;

namespace Rtl.TvMaze.Scraper.Service.Contracts.Model.Cast
{
    public class CastPersonModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("birthday")]
        public string Birthday { get; set; }
    }
}
