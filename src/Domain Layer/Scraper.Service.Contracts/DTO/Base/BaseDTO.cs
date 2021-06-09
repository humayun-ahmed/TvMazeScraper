namespace Rtl.TvMaze.Scraper.Service.Contracts.DTO.Base
{
    public class BaseDTO<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public void AddError(string error) => ErrorMessage = error;
    }
}
