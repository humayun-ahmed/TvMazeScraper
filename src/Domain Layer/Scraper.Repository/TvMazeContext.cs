using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Rtl.TvMaze.Scraper.Repository.Entity;
using Rtl.TvMaze.Scraper.Repository.EtConfiguration;

namespace Rtl.TvMaze.Scraper.Repository
{
    public class TvMazeContext : BaseContext
    {
        public TvMazeContext(DbContextOptions options) : base(options) { }

        public DbSet<Show> Show { get; set; }
        public DbSet<Cast> Cast { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfiguration(new CastConfiguration());
            modelBuilder.ApplyConfiguration(new ShowConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
