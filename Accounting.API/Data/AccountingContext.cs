using Accounting.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Data
{
    public class AccountingContext : DbContext
    {
        public AccountingContext(DbContextOptions<AccountingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Eventraw> Eventraw { get; set; }
        public virtual DbSet<Current> Current { get; set; }
        public virtual DbSet<CurrentMeta> CurrentMeta { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
            builder.Entity<Login>(entity =>
            {
                entity.HasKey(c => c.LoginName);
                entity.HasAlternateKey(c => c.NormalizedLoginName);
                entity.ToTable("logins");
            });

            builder.Entity<Eventraw>(entity =>
            {
                entity.ToTable("eventraw");

                entity.Property(e => e.InfoJson).HasColumnName("InfoJSON");

                entity.Property(e => e.InfoXml).HasColumnName("InfoXML");
            });

            builder.Entity<Current>(entity =>
            {
                entity.ToTable("current");
            });

            builder.Entity<CurrentMeta>(entity =>
            {
                entity.ToTable("currentmeta");

                entity.HasKey(c => c.MachineName);
            });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        //}
    }

}
