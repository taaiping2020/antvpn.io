using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations
{
    public partial class uniqueforloginname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Logins_LoginName",
                table: "Logins",
                column: "LoginName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Logins_NormalizedLoginName",
                table: "Logins",
                column: "NormalizedLoginName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Logins_LoginName",
                table: "Logins");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Logins_NormalizedLoginName",
                table: "Logins");
        }
    }
}
