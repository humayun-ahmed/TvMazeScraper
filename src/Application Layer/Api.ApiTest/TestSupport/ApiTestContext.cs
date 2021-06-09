using System;
using System.Net.Http;

namespace Rtl.TvMaze.Api.ApiTest.TestSupport
{
	public class ApiTestContext : IDisposable
	{
		private bool _disposedValue;

        public HttpClient Client { get;  }

        private TestServerBuilder TestServerBuilder { get;  }

		public ApiTestContext()
		{
			TestServerBuilder = new TestServerBuilder();
			Client = TestServerBuilder.Client;
        }

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					if (TestServerBuilder != null)
					{
						TestServerBuilder.Dispose();
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
