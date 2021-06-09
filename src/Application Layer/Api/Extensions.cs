using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Rtl.TvMaze.Api
{
    public static class Extensions
    {
        public static T BindSettings<T>(this IConfiguration configuration, string sectionName) where T : class, new()
        {
            T implementation = new T();
            configuration.GetSection(sectionName).Bind(implementation);
            return implementation;
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TvMaze API", Version = "v1.0.0" });
            });
        }
    }
}
