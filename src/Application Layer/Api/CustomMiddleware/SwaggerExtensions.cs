﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Rtl.TvMaze.Api.CustomMiddleware
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TvMaze API", Version = "v1.0.0" });
            });
        }
    }
}
