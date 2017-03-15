using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounting.API.Migrations
{
    public partial class adddefaultvalue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logins",
                table: "Logins");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Logins_LoginName",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Logins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logins",
                table: "Logins",
                column: "LoginName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logins",
                table: "Logins");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Logins",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logins",
                table: "Logins",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Logins_LoginName",
                table: "Logins",
                column: "LoginName");
        }
    }
}
