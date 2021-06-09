using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rtl.TvMaze.Api.ApiTest.TestSupport;
using Rtl.TvMaze.Api.Controllers.V1;

namespace Rtl.TvMaze.Api.ApiTest
{
    [TestClass]
    public class ShowControllerTests : ApiTestBase
    {
        public static HttpClient Client;

        private static TestServerBuilder TestServerBuilder;

        public ShowControllerTests()
        {
            TestServerBuilder = new TestServerBuilder();
            Client = TestServerBuilder.Client;
        }

        [TestMethod]
        public async Task Shows_Succeed()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiRoutes.Shows}?PageNo=1&PageSize=10");

            var response = await Client.SendAsync(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, "Request should be success.");
            
            var result = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(result);
        }

        [ClassCleanup]
        public static void Clean()
        {
            TestServerBuilder.Dispose();
        }
    }
}
