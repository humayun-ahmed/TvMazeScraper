namespace Infrastructure.Repository.Contracts.Test
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	using Models;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using SimpleInjector.Lifestyles;

	[TestClass]
    public class RepositoryTest : TestBase
    {

        public IRepository Repository { get; set; }
        [TestInitialize]
        public void Init()
        {
            this.InitContainer();
        }

        [TestMethod]
        public void AddMustSucceed()
        {
            using (AsyncScopedLifestyle.BeginScope(this.Container))
            {
                this.Repository = this.Container.GetInstance<IRepository>();
                var addedItem = this.Add();
                var categories = this.Repository.GetItems<ProductCategory>(p => p.Id == addedItem.Id);
                Assert.AreEqual(1, categories.Count());
            }
        }

        [TestMethod]
        public void UpdateSucceed()
        {
            using (AsyncScopedLifestyle.BeginScope(this.Container))
            {
                this.Repository = this.Container.GetInstance<IRepository>();
                var addedItem = this.Add();
                addedItem.CategoryName = "Bat";
                this.Repository.Update(addedItem);
                this.Repository.SaveChanges();
                var categories = this.Repository.GetItems<ProductCategory>(p => p.CategoryName == "Bat");
                Assert.AreEqual(1, categories.Count());
            }
        }

        [TestMethod]
        public void DeleteMustSucceed()
        {
            using (AsyncScopedLifestyle.BeginScope(this.Container))
            {
                this.Repository = this.Container.GetInstance<IRepository>();
                var addedItem = this.Add();
                this.Repository.Remove<ProductCategory>(p => p.Id == addedItem.Id);
                this.Repository.SaveChanges();
                var categories = this.Repository.GetItems<ProductCategory>(p => p.Id == addedItem.Id);
                Assert.AreEqual(0, categories.Count());
            }
        }


        [TestMethod]
        public async Task GetItemMustSucceed()
        {
            using (AsyncScopedLifestyle.BeginScope(this.Container))
            {
                this.Repository = this.Container.GetInstance<IRepository>();
                var addedItem = this.Add();
                var item = await this.Repository.GetItem<ProductCategory>(p => p.Id == addedItem.Id);
                Assert.AreEqual(addedItem.Id, item.Id);
            }
        }

        private ProductCategory Add()
        {
            var category = new ProductCategory
            {
                CategoryCode = "2363",
                CategoryName = "BALL",
                Id = Guid.NewGuid()
            };
            this.Repository.Add(category);
            this.Repository.SaveChanges();
            return category;
        }
    }
}
