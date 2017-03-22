using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations
{
    public partial class renamelogintable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logins",
                table: "Logins");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Logins_NormalizedLoginName",
                table: "Logins");

            migrationBuilder.RenameTable(
                name: "Logins",
                newName: "logins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_logins",
                table: "logins",
                column: "LoginName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_logins_NormalizedLoginName",
                table: "logins",
                column: "NormalizedLoginName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_logins",
                table: "logins");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_logins_NormalizedLoginName",
                table: "logins");

            migrationBuilder.RenameTable(
                name: "logins",
                newName: "Logins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logins",
                table: "Logins",
                column: "LoginName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Logins_NormalizedLoginName",
                table: "Logins",
                column: "NormalizedLoginName");
        }
    }
}
