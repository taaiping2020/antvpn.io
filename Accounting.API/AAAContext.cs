using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Accounting.API.Models;

namespace Accounting.API
{
    public partial class AAAContext : DbContext
    {
        public virtual DbSet<Eventraw> Eventraw { get; set; }
        public virtual DbSet<Current> Current { get; set; }
        public virtual DbSet<CurrentMeta> CurrentMeta { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=sqlserver.antvpn.io; Database=AAA;User ID=sa;Password=Woslaodaha0909;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Eventraw>(entity =>
            {
                entity.ToTable("eventraw");

                entity.Property(e => e.InfoJson).HasColumnName("InfoJSON");

                entity.Property(e => e.InfoXml).HasColumnName("InfoXML");
            });

            modelBuilder.Entity<Current>(entity =>
            {
                entity.ToTable("current");
            });

            modelBuilder.Entity<CurrentMeta>(entity =>
            {
                entity.ToTable("currentmeta");

                entity.HasKey(c => c.MachineName);
            });
        }
    }
}