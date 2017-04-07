using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.API.Migrations
{
    public partial class addHealthReportJson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Off",
                table: "servers");

            migrationBuilder.AddColumn<string>(
                name: "HealthReportJson",
                table: "servers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HealthReportJson",
                table: "servers");

            migrationBuilder.AddColumn<bool>(
                name: "Off",
                table: "servers",
                nullable: false,
                defaultValue: false);
        }
    }
}
