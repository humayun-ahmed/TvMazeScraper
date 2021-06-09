using System;

namespace Rtl.TvMaze.Api.ApiTest.TestSupport
{
	public class ApiTestBase : IDisposable
	{
		private bool _disposedValue;

		//public ApiTestContext TestContext { get; set; }

		public ApiTestBase()
		{
			//TestContext = new ApiTestContext();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
                    /*if (TestContext != null) 
					{
						TestContext.Dispose();
						TestContext = null;
					}*/
				}

                _disposedValue = true;
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
