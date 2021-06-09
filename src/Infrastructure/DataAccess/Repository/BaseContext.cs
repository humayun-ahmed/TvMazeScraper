namespace Infrastructure.Repository
{
	using Microsoft.EntityFrameworkCore;

	public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options):base(options)
        {
        }
       
    }
}
