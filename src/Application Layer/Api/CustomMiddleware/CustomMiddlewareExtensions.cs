// Copyright 2021, Rtl.

using Microsoft.AspNetCore.Builder;

namespace Rtl.TvMaze.Api.CustomMiddleware
{
    public static class CustomMiddlewareExtensions
    {
        public static void ConfigureGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }

        public static IApplicationBuilder UserRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogger>();
        }
    }
}