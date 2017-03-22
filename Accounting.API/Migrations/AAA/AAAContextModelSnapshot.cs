using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Accounting.API;
using Extensions;

namespace Accounting.API.Migrations.AAA
{
    [DbContext(typeof(AAAContext))]
    partial class AAAContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Accounting.API.Eventraw", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("InfoJson")
                        .HasColumnName("InfoJSON");

                    b.Property<string>("InfoXml")
                        .HasColumnName("InfoXML");

                    b.HasKey("Id");

                    b.ToTable("eventraw");
                });

            modelBuilder.Entity("Accounting.API.Models.Current", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthMethod");

                    b.Property<int>("Bandwidth");

                    b.Property<string>("ClientExternalAddress");

                    b.Property<string>("ClientIPv4Address");

                    b.Property<TimeSpan?>("ConnectionDuration");

                    b.Property<DateTime?>("ConnectionStartTime");

                    b.Property<int>("ConnectionType");

                    b.Property<string>("MachineName");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<long>("TotalBytesIn");

                    b.Property<long>("TotalBytesOut");

                    b.Property<string>("TransitionTechnology");

                    b.Property<int>("TunnelType");

                    b.Property<int>("UserActivityState");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("current");
                });
        }
    }
}
