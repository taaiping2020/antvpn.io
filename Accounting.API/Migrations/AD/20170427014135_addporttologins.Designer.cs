using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Accounting.API.Data;

namespace Accounting.API.Migrations.AD
{
    [DbContext(typeof(ADContext))]
    [Migration("20170427014135_addporttologins")]
    partial class addporttologins
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("Relational:Sequence:.PortNumbers", "'PortNumbers', '', '20000', '1', '', '65535', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Accounting.API.Models.Login", b =>
                {
                    b.Property<string>("LoginName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256);

                    b.Property<bool>("AllowDialIn");

                    b.Property<bool>("Enabled");

                    b.Property<string>("GroupName")
                        .HasMaxLength(256);

                    b.Property<long?>("MonthlyTraffic");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Port")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEXT VALUE FOR dbo.PortNumbers");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("LoginName");

                    b.ToTable("logins");
                });
        }
    }
}
