using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.API.Migrations.AD
{
    public partial class addporttologins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "PortNumbers",
                startValue: 20000L,
                maxValue: 65535L);

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "logins",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.PortNumbers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "PortNumbers");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "logins");
        }
    }
}
