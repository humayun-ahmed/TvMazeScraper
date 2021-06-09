using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Rtl.TvMaze.Api.ApiTest.TestSupport
{
	/// <summary>
	/// Build a TestServer which intercepts the calls to Auth0 and takes care of the token validation calls to Auth0.
	/// </summary>
	/// <remarks>
	/// Keep in mind that in XUNIT every test gets its own test class instance in isolation.
	/// That means the test class is initiated for every test in that test class.
	/// </remarks>
	internal class TestServerBuilder : IDisposable
	{
		private bool _disposedValue;

		public HttpClient Client { get; set; }

		/// <summary>
		/// The TestServer
		/// </summary>
		public TestServer TestServerHost { get; private set; }

		public TestServerBuilder()
		{
			var projectDir = Directory.GetCurrentDirectory();
			var configPath = Path.Combine(projectDir, "appsettings.json");

			var builder = new WebHostBuilder()
				.UseSerilog()
				.UseStartup<Startup>()
				.ConfigureServices(services =>
				{
					
				})
				.ConfigureAppConfiguration((context, conf) =>
				{
					//load the appsettings for the test run
					//The TestServer is not loading the appsettings from the WebApi project!!!!
					conf.AddJsonFile(configPath);
				});


			TestServerHost = new TestServer(builder);
			Client = TestServerHost.CreateClient();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
                    if (TestServerHost != null)
					{
						TestServerHost.Dispose();
						TestServerHost = null;
					}
				}

                _disposedValue = true;
			}
		}

		public void Dispose()
		{
            Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
