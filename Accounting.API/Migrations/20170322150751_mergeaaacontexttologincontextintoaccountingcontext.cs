using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounting.API.Migrations
{
    public partial class mergeaaacontexttologincontextintoaccountingcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eventraw",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InfoJSON = table.Column<string>(nullable: true),
                    InfoXML = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventraw", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "current",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthMethod = table.Column<string>(nullable: true),
                    Bandwidth = table.Column<int>(nullable: false),
                    ClientExternalAddress = table.Column<string>(nullable: true),
                    ClientIPv4Address = table.Column<string>(nullable: true),
                    ConnectionDuration = table.Column<TimeSpan>(nullable: true),
                    ConnectionStartTime = table.Column<DateTime>(nullable: true),
                    ConnectionType = table.Column<int>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TotalBytesIn = table.Column<long>(nullable: false),
                    TotalBytesOut = table.Column<long>(nullable: false),
                    TransitionTechnology = table.Column<string>(nullable: true),
                    TunnelType = table.Column<int>(nullable: false),
                    UserActivityState = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "currentmeta",
                columns: table => new
                {
                    MachineName = table.Column<string>(maxLength: 100, nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currentmeta", x => x.MachineName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eventraw");

            migrationBuilder.DropTable(
                name: "current");

            migrationBuilder.DropTable(
                name: "currentmeta");
        }
    }
}
