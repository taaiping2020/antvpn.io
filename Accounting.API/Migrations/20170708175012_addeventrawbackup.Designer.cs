using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Accounting.API.Data;
using SharedProject;

namespace Accounting.API.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20170708175012_addeventrawbackup")]
    partial class addeventrawbackup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Accounting.API.Eventraw", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AcctInputOctets");

                    b.Property<long?>("AcctOutputOctets");

                    b.Property<int?>("AcctStatusType");

                    b.Property<DateTime?>("EventTimeStamp");

                    b.Property<string>("InfoJson")
                        .HasColumnName("InfoJSON");

                    b.Property<string>("InfoXml")
                        .HasColumnName("InfoXML");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("eventraw");
                });

            modelBuilder.Entity("Accounting.API.EventrawBackup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AcctInputOctets");

                    b.Property<long?>("AcctOutputOctets");

                    b.Property<int?>("AcctStatusType");

                    b.Property<DateTime?>("EventTimeStamp");

                    b.Property<string>("InfoJson");

                    b.Property<string>("InfoXml");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("EventrawBackup");
                });

            modelBuilder.Entity("Accounting.API.Models.Current", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthMethod");

                    b.Property<int>("Bandwidth");

                    b.Property<string>("ClientExternalAddress");

                    b.Property<string>("ClientIPv4Address");

                    b.Property<long?>("ConnectionDuration");

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

            modelBuilder.Entity("Accounting.API.Models.CurrentMeta", b =>
                {
                    b.Property<string>("MachineName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("MachineName");

                    b.ToTable("currentmeta");
                });

            modelBuilder.Entity("SharedProject.SSEventraw", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MachineName");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<long>("TotalBytesInOut");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("sseventraw");
                });
        }
    }
}
