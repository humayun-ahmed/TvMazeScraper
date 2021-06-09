namespace Infrastructure.Repository.Contracts.Test
{
	using Microsoft.EntityFrameworkCore;

	using SimpleInjector;
	using SimpleInjector.Lifestyles;
	

	public abstract class TestBase
    {
        public Container Container { get; set; }
        public void InitContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Register<BaseContext>(()=> {
                var builder = new DbContextOptionsBuilder();
                builder.UseInMemoryDatabase(databaseName: "Add_writes_to_database");
                return new TestDbContext(builder.Options);
            }, Lifestyle.Scoped);
            container.Register<IRepository, Repository>();
            container.Verify();
            this.Container = container;
        }

    }
}


