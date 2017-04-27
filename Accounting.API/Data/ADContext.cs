
using Accounting.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Data
{
    public class ADContext : DbContext
    {
        public ADContext(DbContextOptions<ADContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Login> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("PortNumbers")
                .StartsAt(20000)
                .HasMax(65535)
                .IncrementsBy(1);

            builder.Entity<Login>(entity =>
            {
                entity.HasKey(c => c.LoginName);
                entity.ToTable("logins");
                entity.Property(b => b.Port)
                      .HasDefaultValueSql("NEXT VALUE FOR dbo.PortNumbers");
            });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
