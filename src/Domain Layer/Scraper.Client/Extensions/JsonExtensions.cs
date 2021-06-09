using System;
using Newtonsoft.Json;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO.Base;

namespace Rtl.TvMaze.Scraper.Service.Extensions
{
    public static class JsonExtensions
    {
        public static BaseDTO<T> TryDeserialize<T>(this string json)
        {
            var result = new BaseDTO<T>();

            try
            {
                result.Data = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                result.AddError($"Failed to deserialize json to type of {typeof(T)} - {ex.Message}");
            }

            return result;
        }
    }
}
