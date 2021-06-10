// Copyright 2021, Rtl.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rtl.TvMaze.Api.CustomMiddleware
{
    public class RequestLogger
    {
        private readonly RequestDelegate m_next;

        public RequestLogger(RequestDelegate next)
        {
            m_next = next;
        }

        public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            await m_next(httpContext);

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