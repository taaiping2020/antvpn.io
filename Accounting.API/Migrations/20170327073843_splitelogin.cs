using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations
{
    public partial class splitelogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logins");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "logins",
                columns: table => new
                {
                    LoginName = table.Column<string>(maxLength: 256, nullable: false),
                    AllowDialIn = table.Column<bool>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    GroupName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedLoginName = table.Column<string>(maxLength: 256, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logins", x => x.LoginName);
                    table.UniqueConstraint("AK_logins_NormalizedLoginName", x => x.NormalizedLoginName);
                });
        }
    }
}
