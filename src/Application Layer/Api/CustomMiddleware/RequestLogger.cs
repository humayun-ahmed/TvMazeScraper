// Copyright 2021, Rtl.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rtl.TvMaze.Api.CustomMiddleware
{
    public class RequestLogger
    {
        private readonly RequestDelegate _next;

        public RequestLogger(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            await _next(httpContext);

            /*var repository = (IRepository)serviceProvider.GetService(typeof(IRepository));

            var requestLog = new RequestLog
            {
                RequestStatusCode = httpContext.Response.StatusCode,
                RequestTime = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                IsException = httpContext.Response.StatusCode==500,
                RequestDetail = httpContext.Request.GetDisplayUrl()
            };
            repository.Add(requestLog);
            await repository.SaveChanges();*/
        }
    }
}