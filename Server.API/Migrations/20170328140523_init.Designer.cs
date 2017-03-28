using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Server.API.Data;

namespace Server.API.Migrations
{
    [DbContext(typeof(ServerContext))]
    [Migration("20170328140523_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Server.API.Models.Server", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Domain");

                    b.Property<string>("IPv4");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Name");

                    b.ToTable("servers");
                });
        }
    }
}
