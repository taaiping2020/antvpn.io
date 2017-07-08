using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounting.API.Migrations
{
    public partial class addeventrawbackup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventrawBackup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcctInputOctets = table.Column<long>(nullable: true),
                    AcctOutputOctets = table.Column<long>(nullable: true),
                    AcctStatusType = table.Column<int>(nullable: true),
                    EventTimeStamp = table.Column<DateTime>(nullable: true),
                    InfoJson = table.Column<string>(nullable: true),
                    InfoXml = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventrawBackup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventrawBackup");
        }
    }
}
