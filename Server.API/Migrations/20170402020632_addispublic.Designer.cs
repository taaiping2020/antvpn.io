﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Server.API.Data;

namespace Server.API.Migrations
{
    [DbContext(typeof(ServerContext))]
    [Migration("20170402020632_addispublic")]
    partial class addispublic
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

                    b.Property<string>("IPv4");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

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
