using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Server.API.Data;

namespace Server.API.Migrations
{
    [DbContext(typeof(ServerContext))]
    [Migration("20170406095729_addHealthReporttable")]
    partial class addHealthReporttable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Server.API.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Flag");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("countries");
                });

            modelBuilder.Entity("Server.API.Models.HealthReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HealthReportJson");

                    b.HasKey("Id");

                    b.ToTable("healthreports");
                });

            modelBuilder.Entity("Server.API.Models.Protocal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("protocals");
                });

            modelBuilder.Entity("Server.API.Models.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CountryId");

                    b.Property<string>("Description");

                    b.Property<string>("Domain");

                    b.Property<string>("HealthReportJson");

                    b.Property<string>("IPv4");

                    b.Property<bool>("IsHybrid");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int?>("RedirectorServerId");

                    b.Property<int?>("TrafficServerId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("RedirectorServerId");

                    b.HasIndex("TrafficServerId");

                    b.ToTable("servers");
                });

            modelBuilder.Entity("Server.API.Models.ServerProtocal", b =>
                {
                    b.Property<int>("ProtocalId");

                    b.Property<int>("ServerId");

                    b.HasKey("ProtocalId", "ServerId");

                    b.HasIndex("ServerId");

                    b.ToTable("serverprotocals");
                });

            modelBuilder.Entity("Server.API.Models.Server", b =>
                {
                    b.HasOne("Server.API.Models.Country", "Country")
                        .WithMany("Servers")
                        .HasForeignKey("CountryId");

                    b.HasOne("Server.API.Models.Server", "RedirectorServer")
                        .WithMany()
                        .HasForeignKey("RedirectorServerId");

                    b.HasOne("Server.API.Models.Server", "TrafficServer")
                        .WithMany()
                        .HasForeignKey("TrafficServerId");
                });

            modelBuilder.Entity("Server.API.Models.ServerProtocal", b =>
                {
                    b.HasOne("Server.API.Models.Protocal", "Protocal")
                        .WithMany("ServerProtocals")
                        .HasForeignKey("ProtocalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Server.API.Models.Server", "Server")
                        .WithMany("ServerProtocals")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
