namespace Rtl.TvMaze.Scraper.Service.Contracts.Settings
{
    public class ScraperSettings
    {
        public string ApiBaseUrl { get; set; }
        public string ShowsRoute { get; set; }
        public string CastRoute { get; set; }
        public int PageSize { get; set; }
    }
}
