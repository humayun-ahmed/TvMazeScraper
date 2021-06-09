using System.Threading.Tasks;
using Infrastructure.Repository.Contracts.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rtl.TvMaze.Scraper.Service.Contracts;
using Rtl.TvMaze.Scraper.Service.Contracts.DTO;

namespace Rtl.TvMaze.Scraper.Service.Test
{
    [TestClass]
    public class ShowQueryServiceTests
    {
        private Mock<IShowQueryService> m_showQueryServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            m_showQueryServiceMock = new Mock<IShowQueryService>();
        }

        [TestMethod]
        public async Task Show_Success()
        {
            var request = new ShowRequest {PageNo = 1, PageSize = 10};
            var result = new PagedResult<ShowDTO>();
            m_showQueryServiceMock.Setup(s => s.GetShows(request)).ReturnsAsync(() => result);

            var show = await m_showQueryServiceMock.Object.GetShows(request);
            m_showQueryServiceMock.Verify(m => m.GetShows(It.IsAny<ShowRequest>()), Times.Once());
        }
    }
}
