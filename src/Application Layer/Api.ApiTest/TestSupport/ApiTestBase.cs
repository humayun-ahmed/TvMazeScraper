using System;

namespace Rtl.TvMaze.Api.ApiTest.TestSupport
{
	public class ApiTestBase : IDisposable
	{
		private bool m_disposedValue;

		//public ApiTestContext TestContext { get; set; }

		public ApiTestBase()
		{
			//TestContext = new ApiTestContext();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!m_disposedValue)
			{
				if (disposing)
				{
                    /*if (TestContext != null) 
					{
						TestContext.Dispose();
						TestContext = null;
					}*/
				}

                m_disposedValue = true;
			}
		}

        public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
