using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rtl.TvMaze.Scraper.Repository.Entity;

namespace Rtl.TvMaze.Scraper.Repository.EtConfiguration
{
    public  class CastConfiguration : IEntityTypeConfiguration<Cast>
    {
        public void Configure(EntityTypeBuilder<Cast> modelBuilder)
        {
            //var entity = modelBuilder.Entity<Cast>();

            // Key
            modelBuilder.HasKey(k => k.Id);

            // Index
            modelBuilder.HasIndex(i => new { i.ShowId, i.Name })
                .IsUnique();

            // Relations
            modelBuilder.HasOne(p => p.Show)
                  .WithMany(o => o.Casts)
                  .HasForeignKey(f => f.ShowId);

            // Length
            modelBuilder.Property(p => p.Name).HasMaxLength(1024);
        }
    }
}
