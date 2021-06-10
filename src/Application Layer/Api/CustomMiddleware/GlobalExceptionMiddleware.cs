// Copyright 2021, Rtl.

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Rtl.TvMaze.Api.CustomMiddleware
{
    /// <summary>
    ///     // Catch all unexpected unhandled exceptions and make sure a nice message is returned with status code 500.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly ILogger m_logger;
        private readonly RequestDelegate m_next;

        public GlobalExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<GlobalExceptionMiddleware>();
            m_next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await m_next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            // log the raw exception
            m_logger.LogError(exception, "Unexpected unhandled error in Tv Maze Service.");

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "Oops. Something went wrong."
            }.ToString());
        }
    }
}