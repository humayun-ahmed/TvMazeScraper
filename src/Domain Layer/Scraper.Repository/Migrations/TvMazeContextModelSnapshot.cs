﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rtl.TvMaze.Scraper.Repository;

namespace Rtl.TvMaze.Scraper.Repository.Migrations
{
    [DbContext(typeof(TvMazeContext))]
    partial class TvMazeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Rtl.TvMaze.Scraper.Repository.Entity.Cast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("ShowId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShowId", "Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Cast");
                });

            modelBuilder.Entity("Rtl.TvMaze.Scraper.Repository.Entity.Show", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Show");
                });

            modelBuilder.Entity("Rtl.TvMaze.Scraper.Repository.Entity.Cast", b =>
                {
                    b.HasOne("Rtl.TvMaze.Scraper.Repository.Entity.Show", "Show")
                        .WithMany("Casts")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Show");
                });

            modelBuilder.Entity("Rtl.TvMaze.Scraper.Repository.Entity.Show", b =>
                {
                    b.Navigation("Casts");
                });
#pragma warning restore 612, 618
        }
    }
}
