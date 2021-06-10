using System;
using System.Net.Http;

namespace Rtl.TvMaze.Api.ApiTest.TestSupport
{
	public class ApiTestContext : IDisposable
	{
		private bool m_disposedValue;

        public HttpClient Client { get;  }

        private TestServerBuilder TestServerBuilder { get;  }

		public ApiTestContext()
		{
			TestServerBuilder = new TestServerBuilder();
			Client = TestServerBuilder.Client;
        }

		protected virtual void Dispose(bool disposing)
		{
			if (!m_disposedValue)
			{
				if (disposing)
				{
					if (TestServerBuilder != null)
					{
						TestServerBuilder.Dispose();
					}
                }
                m_disposedValue = true;
			}
		}

        public void Dispose()
		{
            Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
