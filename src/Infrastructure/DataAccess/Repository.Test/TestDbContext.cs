namespace Infrastructure.Repository.Contracts.Test
{
	using Models;

	using Microsoft.EntityFrameworkCore;

	public class TestDbContext : BaseContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
