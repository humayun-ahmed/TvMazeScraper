using Newtonsoft.Json;

namespace Rtl.TvMaze.Scraper.Service.Contracts.Model.Cast
{
    public class CastModel
    {
        [JsonProperty("person")]
        public CastPersonModel Person { get; set; }
    }
}
