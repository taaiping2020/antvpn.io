using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Accounting.API.Data;

namespace Accounting.API.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20170314063538_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Accounting.API.Models.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowDialIn");

                    b.Property<bool>("Enabled");

                    b.Property<string>("GroupName")
                        .HasMaxLength(256);

                    b.Property<string>("LoginName")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedLoginName")
                        .HasMaxLength(256);

                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("Logins");
                });
        }
    }
}
