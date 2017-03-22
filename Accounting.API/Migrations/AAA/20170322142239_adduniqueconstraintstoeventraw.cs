using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations.AAA
{
    public partial class adduniqueconstraintstoeventraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InfoXML",
                table: "eventraw",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InfoJSON",
                table: "eventraw",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_eventraw_InfoJSON",
                table: "eventraw",
                column: "InfoJSON");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_eventraw_InfoXML",
                table: "eventraw",
                column: "InfoXML");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_eventraw_InfoJSON",
                table: "eventraw");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_eventraw_InfoXML",
                table: "eventraw");

            migrationBuilder.AlterColumn<string>(
                name: "InfoXML",
                table: "eventraw",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "InfoJSON",
                table: "eventraw",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
