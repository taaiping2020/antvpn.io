using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Accounting.API.Data;

namespace Accounting.API.Migrations
{
    [DbContext(typeof(LoginContext))]
    partial class LoginContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedLoginName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Password")
                        .HasMaxLength(100);

                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasAlternateKey("LoginName");


                    b.HasAlternateKey("NormalizedLoginName");

                    b.ToTable("Logins");
                });
        }
    }
}
