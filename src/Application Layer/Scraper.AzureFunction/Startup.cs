using System;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Rtl.TvMaze.Scraper.AzureFunction;
using Rtl.TvMaze.Scraper.Service;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Rtl.TvMaze.Scraper.AzureFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
             var configuration = configurationBuilder.Build();

             builder.Services.AddDependencies(configuration);
        }
    }
}
