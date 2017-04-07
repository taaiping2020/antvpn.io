using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.API.Models;

namespace Server.API.Data
{
    public class ServerContext : DbContext
    {
        public ServerContext(DbContextOptions<ServerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Server.API.Models.Server> Servers { get; set; }
        public virtual DbSet<Server.API.Models.Protocal> Protocals { get; set; }
        public virtual DbSet<Server.API.Models.Country> Countries { get; set; }
        public virtual DbSet<Server.API.Models.HealthReport> HealthReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Server.API.Models.Server>(entity =>
            {
                //entity.HasKey(c => c.Name);
                entity.ToTable("servers");

                entity.HasOne(c => c.RedirectorServer).WithMany().HasForeignKey(c => c.RedirectorServerId);
                entity.HasOne(c => c.TrafficServer).WithMany().HasForeignKey(c => c.TrafficServerId);
            });

            builder.Entity<Server.API.Models.Protocal>(entity =>
            {
                //entity.HasKey(c => c.Name);
                entity.ToTable("protocals");
            });

            builder.Entity<Server.API.Models.Country>(entity =>
            {
                //entity.HasKey(c => c.Name);
                entity.ToTable("countries");
            });

            builder.Entity<Server.API.Models.HealthReport>(entity =>
            {
                //entity.HasKey(c => c.Name);
                entity.ToTable("healthreports");
            });

            builder.Entity<Server.API.Models.ServerProtocal>(entity =>
            {
                entity.HasKey(u => new { u.ProtocalId, u.ServerId });
                entity.ToTable("serverprotocals");
            });

          
            builder.Entity<ServerProtocal>()
                .HasOne(sc => sc.Protocal)
                .WithMany(s => s.ServerProtocals)
                .HasForeignKey(sc => sc.ProtocalId);

            builder.Entity<ServerProtocal>()
               .HasOne(sc => sc.Server)
               .WithMany(s => s.ServerProtocals)
               .HasForeignKey(sc => sc.ServerId);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
