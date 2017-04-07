using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Accounting.API.Data;

namespace Accounting.API.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20170322150751_mergeaaacontexttologincontextintoaccountingcontext")]
    partial class mergeaaacontexttologincontextintoaccountingcontext
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

            modelBuilder.Entity("Accounting.API.Models.CurrentMeta", b =>
                {
                    b.Property<string>("MachineName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("MachineName");

                    b.ToTable("currentmeta");
                });

            modelBuilder.Entity("Accounting.API.Models.Login", b =>
                {
                    b.Property<string>("LoginName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256);

                    b.Property<bool>("AllowDialIn");

                    b.Property<bool>("Enabled");

                    b.Property<string>("GroupName")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedLoginName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("LoginName");

                    b.HasAlternateKey("NormalizedLoginName");

                    b.ToTable("Logins");
                });
        }
    }
}
