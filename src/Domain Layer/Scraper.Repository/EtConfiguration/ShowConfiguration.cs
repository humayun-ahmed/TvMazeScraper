using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rtl.TvMaze.Scraper.Repository.Entity;

namespace Rtl.TvMaze.Scraper.Repository.EtConfiguration
{
    public  class ShowConfiguration : IEntityTypeConfiguration<Show>
    {
        public void Configure(EntityTypeBuilder<Show> modelBuilder)
        {
            // Key
            modelBuilder.HasKey(k => k.Id);

            // Index
            modelBuilder.HasIndex(i => i.Name)
                  .IsUnique();

            // Relations
            modelBuilder.HasMany(p => p.Casts)
                  .WithOne(o => o.Show)
                  .HasForeignKey(f => f.ShowId);

            // Length
            modelBuilder.Property(p => p.Name).HasMaxLength(1024);
        }
    }
}
