// Copyright 2021, Rtl.

using Newtonsoft.Json;

namespace Rtl.TvMaze.Api.CustomMiddleware
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}