using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations
{
    public partial class addpropstoeventraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AcctInputOctets",
                table: "eventraw",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AcctOutputOctets",
                table: "eventraw",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "AcctStatusType",
                table: "eventraw",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventTimeStamp",
                table: "eventraw",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "eventraw",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcctInputOctets",
                table: "eventraw");

            migrationBuilder.DropColumn(
                name: "AcctOutputOctets",
                table: "eventraw");

            migrationBuilder.DropColumn(
                name: "AcctStatusType",
                table: "eventraw");

            migrationBuilder.DropColumn(
                name: "EventTimeStamp",
                table: "eventraw");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "eventraw");
        }
    }
}
