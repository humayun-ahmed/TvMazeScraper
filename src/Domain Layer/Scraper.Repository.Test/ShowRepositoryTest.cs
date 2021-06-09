using System;
using System.Threading.Tasks;
using Infrastructure.Repository.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rtl.TvMaze.Scraper.Repository.Entity;

namespace Scraper.Repository.Test
{
    [TestClass]
    public class ShowRepositoryTest
    {
        private Mock<IReadOnlyRepository> m_repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            m_repositoryMock = new Mock<IReadOnlyRepository>();
        }

        [TestMethod]
        public async Task Show_AnyAsync()
        {
            // Arrange
            m_repositoryMock.Setup(s => s.AnyAsync<Show>(a => a.Name == Guid.NewGuid().ToString())).ReturnsAsync(() => true);

            // Act
            var show = await m_repositoryMock.Object.AnyAsync<Show>(a => a.Name == Guid.NewGuid().ToString());

            // Assert
            m_repositoryMock.Verify(m => m.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Show, bool>>>()), Times.Once());
        }
    }
}
